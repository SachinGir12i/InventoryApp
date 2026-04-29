using InventoryApp.Application.Features.Sales.DTOs;
using InventoryApp.Application.Interfaces;
using InventoryApp.Domain.Entities;
using InventoryApp.Domain.Enums;
using MediatR;

namespace InventoryApp.Application.Features.Sales.Commands
{
    // ── The Command ──────────────────────────────────────────────
    public class CreateSaleCommand : IRequest<int>
    {
        public CreateSaleDto Sale { get; set; } = null!;
    }

    // ── The Handler ──────────────────────────────────────────────
    public class CreateSaleCommandHandler
        : IRequestHandler<CreateSaleCommand, int>
    {
        private readonly ISaleRepository _saleRepo;
        private readonly IItemRepository _itemRepo;

        public CreateSaleCommandHandler(
            ISaleRepository saleRepo,
            IItemRepository itemRepo)
        {
            _saleRepo = saleRepo;
            _itemRepo = itemRepo;
        }

        public async Task<int> Handle(
            CreateSaleCommand request,
            CancellationToken cancellationToken)
        {
            var dto = request.Sale;

            // ── Step 1: Validate stock availability ──────────────
            // Before creating the sale, check if we have
            // enough stock for each item being sold
            foreach (var saleItem in dto.Items)
            {
                var item = await _itemRepo.GetByIdAsync(saleItem.ItemId);

                if (item == null)
                    throw new Exception(
                        $"Item with Id {saleItem.ItemId} not found");

                // Cannot sell more than what is in stock
                if (item.CurrentStock < saleItem.Quantity)
                    throw new Exception(
                        $"Insufficient stock for '{item.Name}'. " +
                        $"Available: {item.CurrentStock}, " +
                        $"Requested: {saleItem.Quantity}");
            }

            // ── Step 2: Calculate line totals ────────────────────
            decimal subTotal = dto.Items
                .Sum(i => (i.Quantity * i.UnitPrice) - i.LineDiscount);

            // ── Step 3: Apply overall discount ───────────────────
            decimal afterDiscount = subTotal - dto.DiscountAmount;

            // ── Step 4: Calculate VAT ────────────────────────────
            decimal vatAmount = afterDiscount * (dto.VATPercent / 100);

            // ── Step 5: Calculate grand total ────────────────────
            decimal totalAmount = afterDiscount + vatAmount;

            // ── Step 6: Calculate due amount ─────────────────────
            decimal dueAmount = totalAmount - dto.ReceivedAmount;

            // ── Step 7: Determine payment status ─────────────────
            var paymentStatus = dto.ReceivedAmount <= 0
                ? PaymentStatus.Unpaid
                : dto.ReceivedAmount >= totalAmount
                    ? PaymentStatus.FullyPaid
                    : PaymentStatus.PartiallyPaid;

            // ── Step 8: Generate sale number ─────────────────────
            var saleNumber = await _saleRepo.GenerateSaleNumberAsync();

            // ── Step 9: Build the Sale entity ────────────────────
            var sale = new Sale
            {
                SaleNumber = saleNumber,
                SaleDate = dto.SaleDate,
                CustomerId = dto.CustomerId,
                SubTotal = subTotal,
                DiscountAmount = dto.DiscountAmount,
                VATPercent = dto.VATPercent,
                VATAmount = vatAmount,
                TotalAmount = totalAmount,
                ReceivedAmount = dto.ReceivedAmount,
                DueAmount = dueAmount,
                PaymentStatus = paymentStatus,
                Remarks = dto.Remarks,

                // Build line items
                SaleItems = dto.Items.Select(i => new SaleItem
                {
                    ItemId = i.ItemId,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    PriceType = i.PriceType,
                    LineDiscount = i.LineDiscount,
                    LineTotal = (i.Quantity * i.UnitPrice) - i.LineDiscount
                }).ToList()
            };

            // ── Step 10: Save sale + decrease stock ──────────────
            var created = await _saleRepo.CreateAsync(sale);
            return created.Id;
        }
    }
}