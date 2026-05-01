namespace InventoryApp.Application.Features.Productions.DTOs
{
    // ── Each raw material being consumed ─────────────────────────
    public class ProductionMaterialDto
    {
        // Which raw material (e.g. Leather item id)
        public int ItemId { get; set; }

        // How much is being used
        public decimal QuantityUsed { get; set; }

        // Cost per unit of this material
        public decimal UnitCost { get; set; }
    }

    // ── Each finished shoe being produced ────────────────────────
    public class ProductionOutputDto
    {
        // Which finished shoe (e.g. Shoes Size 6 item id)
        public int ItemId { get; set; }

        // How many pairs being produced
        public decimal QuantityProduced { get; set; }
    }

    // ── The full production request ──────────────────────────────
    public class CreateProductionDto
    {
        public DateTime ProductionDate { get; set; } = DateTime.UtcNow;

        // Brief description e.g. "Morning batch - casual shoes"
        public string? Description { get; set; }

        // Worker wages for this batch
        public decimal LaborCost { get; set; } = 0;

        // Electricity, tools, miscellaneous
        public decimal OtherCost { get; set; } = 0;

        public string? Remarks { get; set; }

        // List of raw materials being consumed
        public List<ProductionMaterialDto> MaterialsUsed { get; set; } = new();

        // List of finished shoes being produced
        public List<ProductionOutputDto> OutputItems { get; set; } = new();
    }
}