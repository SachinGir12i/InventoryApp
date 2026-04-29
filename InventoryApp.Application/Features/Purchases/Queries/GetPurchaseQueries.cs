using InventoryApp.Application.Features.Purchases.DTOs;
using InventoryApp.Application.Interfaces;
using MediatR;

namespace InventoryApp.Application.Features.Purchases.Queries
{
    // ── Helper to map Purchase entity to DTO ─────────────────────
    internal static class PurchaseMapper
    {
        public static PurchaseResponseDto ToDto(Domain.Entities.Purchase p) => new()
        {
            Id = p.Id,
            PurchaseNumber = p.PurchaseNumber,
            PurchaseDate = p.PurchaseDate,
            SupplierId = p.SupplierId,
            SupplierName = p.Supplier?.Name ?? string.Empty,
            SubTotal = p.SubTotal,
            VATPercent = p.VATPercent,
            VATAmount = p.VATAmount,
            TotalAmount = p.TotalAmount,
            PaidAmount = p.PaidAmount,
            DueAmount = p.DueAmount,
            PaymentStatus = p.PaymentStatus.ToString(),
            SupplierInvoiceNumber = p.SupplierInvoiceNumber,
            Remarks = p.Remarks,
            CreatedAt = p.CreatedAt,

            // Map each line item
            Items = p.PurchaseItems.Select(i => new PurchaseItemResponseDto
            {
                Id = i.Id,
                ItemId = i.ItemId,
                ItemName = i.Item?.Name ?? string.Empty,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice,
                LineTotal = i.LineTotal
            }).ToList()
        };
    }

    // ── Get All Purchases ────────────────────────────────────────
    public class GetAllPurchasesQuery : IRequest<List<PurchaseResponseDto>> { }

    public class GetAllPurchasesQueryHandler
        : IRequestHandler<GetAllPurchasesQuery, List<PurchaseResponseDto>>
    {
        private readonly IPurchaseRepository _repo;
        public GetAllPurchasesQueryHandler(IPurchaseRepository repo) => _repo = repo;

        public async Task<List<PurchaseResponseDto>> Handle(
            GetAllPurchasesQuery request, CancellationToken cancellationToken)
        {
            var purchases = await _repo.GetAllAsync();
            return purchases.Select(PurchaseMapper.ToDto).ToList();
        }
    }

    // ── Get Purchase By Id ───────────────────────────────────────
    public class GetPurchaseByIdQuery : IRequest<PurchaseResponseDto?>
    {
        public int Id { get; set; }
    }

    public class GetPurchaseByIdQueryHandler
        : IRequestHandler<GetPurchaseByIdQuery, PurchaseResponseDto?>
    {
        private readonly IPurchaseRepository _repo;
        public GetPurchaseByIdQueryHandler(IPurchaseRepository repo) => _repo = repo;

        public async Task<PurchaseResponseDto?> Handle(
            GetPurchaseByIdQuery request, CancellationToken cancellationToken)
        {
            var purchase = await _repo.GetByIdAsync(request.Id);
            return purchase == null ? null : PurchaseMapper.ToDto(purchase);
        }
    }

    // ── Get Purchases By Supplier ────────────────────────────────
    public class GetPurchasesBySupplierQuery : IRequest<List<PurchaseResponseDto>>
    {
        public int SupplierId { get; set; }
    }

    public class GetPurchasesBySupplierQueryHandler
        : IRequestHandler<GetPurchasesBySupplierQuery, List<PurchaseResponseDto>>
    {
        private readonly IPurchaseRepository _repo;
        public GetPurchasesBySupplierQueryHandler(IPurchaseRepository repo) => _repo = repo;

        public async Task<List<PurchaseResponseDto>> Handle(
            GetPurchasesBySupplierQuery request, CancellationToken cancellationToken)
        {
            var purchases = await _repo.GetBySupplierIdAsync(request.SupplierId);
            return purchases.Select(PurchaseMapper.ToDto).ToList();
        }
    }
}