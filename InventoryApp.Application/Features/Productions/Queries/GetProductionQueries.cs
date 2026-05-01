using InventoryApp.Application.Features.Productions.DTOs;
using InventoryApp.Application.Interfaces;
using MediatR;

namespace InventoryApp.Application.Features.Productions.Queries
{
    // ── Helper mapper ────────────────────────────────────────────
    internal static class ProductionMapper
    {
        public static ProductionResponseDto ToDto(
            Domain.Entities.Production p) => new()
            {
                Id = p.Id,
                ProductionNumber = p.ProductionNumber,
                ProductionDate = p.ProductionDate,
                Description = p.Description,
                RawMaterialCost = p.RawMaterialCost,
                LaborCost = p.LaborCost,
                OtherCost = p.OtherCost,
                TotalCost = p.TotalCost,
                TotalQuantityProduced = p.TotalQuantityProduced,
                CostPerUnit = p.CostPerUnit,
                Remarks = p.Remarks,
                CreatedAt = p.CreatedAt,

                // Map raw materials consumed
                MaterialsUsed = p.MaterialsUsed.Select(m =>
                    new ProductionMaterialResponseDto
                    {
                        Id = m.Id,
                        ItemId = m.ItemId,
                        ItemName = m.Item?.Name ?? string.Empty,
                        QuantityUsed = m.QuantityUsed,
                        UnitCost = m.UnitCost,
                        TotalCost = m.TotalCost
                    }).ToList(),

                // Map finished shoes produced
                OutputItems = p.OutputItems.Select(o =>
                    new ProductionOutputResponseDto
                    {
                        Id = o.Id,
                        ItemId = o.ItemId,
                        ItemName = o.Item?.Name ?? string.Empty,
                        QuantityProduced = o.QuantityProduced,
                        UnitCost = o.UnitCost
                    }).ToList()
            };
    }

    // ── Get All Productions ──────────────────────────────────────
    public class GetAllProductionsQuery
        : IRequest<List<ProductionResponseDto>>
    { }

    public class GetAllProductionsQueryHandler
        : IRequestHandler<GetAllProductionsQuery, List<ProductionResponseDto>>
    {
        private readonly IProductionRepository _repo;
        public GetAllProductionsQueryHandler(IProductionRepository repo)
            => _repo = repo;

        public async Task<List<ProductionResponseDto>> Handle(
            GetAllProductionsQuery request,
            CancellationToken cancellationToken)
        {
            var productions = await _repo.GetAllAsync();
            return productions.Select(ProductionMapper.ToDto).ToList();
        }
    }

    // ── Get Production By Id ─────────────────────────────────────
    public class GetProductionByIdQuery : IRequest<ProductionResponseDto?>
    {
        public int Id { get; set; }
    }

    public class GetProductionByIdQueryHandler
        : IRequestHandler<GetProductionByIdQuery, ProductionResponseDto?>
    {
        private readonly IProductionRepository _repo;
        public GetProductionByIdQueryHandler(IProductionRepository repo)
            => _repo = repo;

        public async Task<ProductionResponseDto?> Handle(
            GetProductionByIdQuery request,
            CancellationToken cancellationToken)
        {
            var production = await _repo.GetByIdAsync(request.Id);
            return production == null
                ? null
                : ProductionMapper.ToDto(production);
        }
    }
}