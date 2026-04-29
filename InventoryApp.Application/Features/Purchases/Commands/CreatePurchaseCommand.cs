using InventoryApp.Application.Features.Purchases.DTOs;
using InventoryApp.Application.Interfaces;
using InventoryApp.Domain.Entities;
using InventoryApp.Domain.Enums;
using MediatR;

namespace InventoryApp.Application.Features.Purchases.Commands
{
    // ── The Command (what we want to do) ─────────────────────────
    public class CreatePurchaseCommand : IRequest<int>
    {
        public CreatePurchaseDto Purchase { get; set; } = null!;
    }

    // ── The Handler (how we do it) ───────────────────────────────
    public class CreatePurchaseCommandHandler
        : IRequestHandler<CreatePurchaseCommand, int>
    {
        private readonly IPurchaseRepository _purchaseRepo;
        private readonly IItemRepository _itemRepo;

        public CreatePurchaseCommandHandler(
            IPurchaseRepository purchaseRepo,
            IItemRepository itemRepo)
        {
            _purchaseRepo = purchaseRepo;
            _itemRepo = itemRepo;
        }

        public async Task<int> Handle(
            CreatePurchaseCommand request,
            CancellationToken cancellationToken)
        {
            var dto = request.Purchase;

            // ── Step 1: Calculate financial totals ───────────────

            // Calculate each line total and sum up
            decimal subTotal = dto.Items
                .Sum(i => i.Quantity * i.UnitPrice);

            // Calculate VAT amount from percentage
            decimal vatAmount = subTotal * (dto.VATPercent / 100);

            // Grand total
            decimal totalAmount = subTotal + vatAmount;

            // How much still owed to supplier
            decimal dueAmount = totalAmount - dto.PaidAmount;

            // ── Step 2: Determine payment status ─────────────────
            var paymentStatus = dto.PaidAmount <= 0
                ? PaymentStatus.Unpaid
                : dto.PaidAmount >= totalAmount
                    ? PaymentStatus.FullyPaid
                    : PaymentStatus.PartiallyPaid;

            // ── Step 3: Generate purchase number ─────────────────
            var purchaseNumber = await _purchaseRepo
                .GeneratePurchaseNumberAsync();

            // ── Step 4: Build the Purchase entity ────────────────
            var purchase = new Purchase
            {
                PurchaseNumber = purchaseNumber,
                PurchaseDate = dto.PurchaseDate,
                SupplierId = dto.SupplierId,
                SubTotal = subTotal,
                VATPercent = dto.VATPercent,
                VATAmount = vatAmount,
                TotalAmount = totalAmount,
                PaidAmount = dto.PaidAmount,
                DueAmount = dueAmount,
                PaymentStatus = paymentStatus,
                SupplierInvoiceNumber = dto.SupplierInvoiceNumber,
                Remarks = dto.Remarks,

                // Build line items from DTO
                PurchaseItems = dto.Items.Select(i => new PurchaseItem
                {
                    ItemId = i.ItemId,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    LineTotal = i.Quantity * i.UnitPrice
                }).ToList()
            };

            // ── Step 5: Save purchase + update stock ─────────────
            // The repository handles both saving the purchase
            // AND increasing stock for each raw material
            var created = await _purchaseRepo.CreateAsync(purchase);

            return created.Id;
        }
    }
}