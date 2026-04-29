using InventoryApp.Application.Features.Sales.DTOs;
using InventoryApp.Application.Interfaces;
using MediatR;

namespace InventoryApp.Application.Features.Sales.Queries
{
    // ── Helper mapper ────────────────────────────────────────────
    internal static class SaleMapper
    {
        public static SaleResponseDto ToDto(Domain.Entities.Sale s) => new()
        {
            Id = s.Id,
            SaleNumber = s.SaleNumber,
            SaleDate = s.SaleDate,
            CustomerId = s.CustomerId,
            CustomerName = s.Customer?.Name ?? string.Empty,
            SubTotal = s.SubTotal,
            DiscountAmount = s.DiscountAmount,
            VATPercent = s.VATPercent,
            VATAmount = s.VATAmount,
            TotalAmount = s.TotalAmount,
            ReceivedAmount = s.ReceivedAmount,
            DueAmount = s.DueAmount,
            PaymentStatus = s.PaymentStatus.ToString(),
            Remarks = s.Remarks,
            CreatedAt = s.CreatedAt,

            // Map each line item
            Items = s.SaleItems.Select(i => new SaleItemResponseDto
            {
                Id = i.Id,
                ItemId = i.ItemId,
                ItemName = i.Item?.Name ?? string.Empty,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice,
                PriceType = i.PriceType.ToString(),
                LineDiscount = i.LineDiscount,
                LineTotal = i.LineTotal
            }).ToList()
        };
    }

    // ── Get All Sales ────────────────────────────────────────────
    public class GetAllSalesQuery : IRequest<List<SaleResponseDto>> { }

    public class GetAllSalesQueryHandler
        : IRequestHandler<GetAllSalesQuery, List<SaleResponseDto>>
    {
        private readonly ISaleRepository _repo;
        public GetAllSalesQueryHandler(ISaleRepository repo) => _repo = repo;

        public async Task<List<SaleResponseDto>> Handle(
            GetAllSalesQuery request, CancellationToken cancellationToken)
        {
            var sales = await _repo.GetAllAsync();
            return sales.Select(SaleMapper.ToDto).ToList();
        }
    }

    // ── Get Sale By Id ───────────────────────────────────────────
    public class GetSaleByIdQuery : IRequest<SaleResponseDto?>
    {
        public int Id { get; set; }
    }

    public class GetSaleByIdQueryHandler
        : IRequestHandler<GetSaleByIdQuery, SaleResponseDto?>
    {
        private readonly ISaleRepository _repo;
        public GetSaleByIdQueryHandler(ISaleRepository repo) => _repo = repo;

        public async Task<SaleResponseDto?> Handle(
            GetSaleByIdQuery request, CancellationToken cancellationToken)
        {
            var sale = await _repo.GetByIdAsync(request.Id);
            return sale == null ? null : SaleMapper.ToDto(sale);
        }
    }

    // ── Get Sales By Customer ────────────────────────────────────
    public class GetSalesByCustomerQuery : IRequest<List<SaleResponseDto>>
    {
        public int CustomerId { get; set; }
    }

    public class GetSalesByCustomerQueryHandler
        : IRequestHandler<GetSalesByCustomerQuery, List<SaleResponseDto>>
    {
        private readonly ISaleRepository _repo;
        public GetSalesByCustomerQueryHandler(ISaleRepository repo) => _repo = repo;

        public async Task<List<SaleResponseDto>> Handle(
            GetSalesByCustomerQuery request, CancellationToken cancellationToken)
        {
            var sales = await _repo.GetByCustomerIdAsync(request.CustomerId);
            return sales.Select(SaleMapper.ToDto).ToList();
        }
    }
}