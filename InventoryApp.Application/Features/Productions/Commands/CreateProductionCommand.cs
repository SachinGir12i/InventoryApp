using InventoryApp.Application.Features.Productions.DTOs;
using InventoryApp.Application.Interfaces;
using InventoryApp.Domain.Entities;
using MediatR;

namespace InventoryApp.Application.Features.Productions.Commands
{
    // ── The Command ──────────────────────────────────────────────
    public class CreateProductionCommand : IRequest<int>
    {
        public CreateProductionDto Production { get; set; } = null!;
    }

    // ── The Handler ──────────────────────────────────────────────
    public class CreateProductionCommandHandler
        : IRequestHandler<CreateProductionCommand, int>
    {
        private readonly IProductionRepository _productionRepo;
        private readonly IItemRepository _itemRepo;

        public CreateProductionCommandHandler(
            IProductionRepository productionRepo,
            IItemRepository itemRepo)
        {
            _productionRepo = productionRepo;
            _itemRepo = itemRepo;
        }

        public async Task<int> Handle(
            CreateProductionCommand request,
            CancellationToken cancellationToken)
        {
            var dto = request.Production;

            // ── Step 1: Validate raw material stock ──────────────
            // Check we have enough raw materials before starting
            foreach (var material in dto.MaterialsUsed)
            {
                var item = await _itemRepo.GetByIdAsync(material.ItemId);

                if (item == null)
                    throw new Exception(
                        $"Raw material with Id {material.ItemId} not found");

                // Cannot use more than what is in stock
                if (item.CurrentStock < material.QuantityUsed)
                    throw new Exception(
                        $"Insufficient stock for '{item.Name}'. " +
                        $"Available: {item.CurrentStock}, " +
                        $"Required: {material.QuantityUsed}");
            }

            // ── Step 2: Calculate raw material cost ──────────────
            decimal rawMaterialCost = dto.MaterialsUsed
                .Sum(m => m.QuantityUsed * m.UnitCost);

            // ── Step 3: Calculate total production cost ──────────
            decimal totalCost = rawMaterialCost
                + dto.LaborCost
                + dto.OtherCost;

            // ── Step 4: Calculate total quantity produced ─────────
            decimal totalQuantityProduced = dto.OutputItems
                .Sum(o => o.QuantityProduced);

            // ── Step 5: Calculate cost per unit ──────────────────
            // This tells you exactly how much one pair of shoes costs
            // to make — very useful for setting your selling price
            decimal costPerUnit = totalQuantityProduced > 0
                ? totalCost / totalQuantityProduced
                : 0;

            // ── Step 6: Generate production number ───────────────
            var productionNumber = await _productionRepo
                .GenerateProductionNumberAsync();

            // ── Step 7: Build Production entity ──────────────────
            var production = new Production
            {
                ProductionNumber = productionNumber,
                ProductionDate = dto.ProductionDate,
                Description = dto.Description,
                RawMaterialCost = rawMaterialCost,
                LaborCost = dto.LaborCost,
                OtherCost = dto.OtherCost,
                TotalCost = totalCost,
                TotalQuantityProduced = totalQuantityProduced,
                CostPerUnit = costPerUnit,
                Remarks = dto.Remarks,

                // Build materials consumed list
                MaterialsUsed = dto.MaterialsUsed.Select(m =>
                    new ProductionMaterial
                    {
                        ItemId = m.ItemId,
                        QuantityUsed = m.QuantityUsed,
                        UnitCost = m.UnitCost,
                        TotalCost = m.QuantityUsed * m.UnitCost
                    }).ToList(),

                // Build output items list
                OutputItems = dto.OutputItems.Select(o =>
                    new ProductionOutput
                    {
                        ItemId = o.ItemId,
                        QuantityProduced = o.QuantityProduced,
                        // Each output item gets the same cost per unit
                        UnitCost = costPerUnit
                    }).ToList()
            };

            // ── Step 8: Save + update stock ───────────────────────
            // Repository handles:
            // - raw material stock going DOWN
            // - finished shoe stock going UP
            var created = await _productionRepo.CreateAsync(production);
            return created.Id;
        }
    }
}