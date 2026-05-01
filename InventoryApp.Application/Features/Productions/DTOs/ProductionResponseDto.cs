namespace InventoryApp.Application.Features.Productions.DTOs
{
    // ── Material used in response ────────────────────────────────
    public class ProductionMaterialResponseDto
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public decimal QuantityUsed { get; set; }
        public decimal UnitCost { get; set; }
        public decimal TotalCost { get; set; }
    }

    // ── Output item in response ──────────────────────────────────
    public class ProductionOutputResponseDto
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public decimal QuantityProduced { get; set; }
        public decimal UnitCost { get; set; }
    }

    // ── Full production response ─────────────────────────────────
    public class ProductionResponseDto
    {
        public int Id { get; set; }
        public string ProductionNumber { get; set; } = string.Empty;
        public DateTime ProductionDate { get; set; }
        public string? Description { get; set; }

        // Cost breakdown
        public decimal RawMaterialCost { get; set; }
        public decimal LaborCost { get; set; }
        public decimal OtherCost { get; set; }
        public decimal TotalCost { get; set; }
        public decimal TotalQuantityProduced { get; set; }

        // This tells you exactly how much it costs to make one pair
        public decimal CostPerUnit { get; set; }

        public string? Remarks { get; set; }
        public DateTime CreatedAt { get; set; }

        // Raw materials consumed
        public List<ProductionMaterialResponseDto> MaterialsUsed { get; set; }
            = new();

        // Finished shoes produced
        public List<ProductionOutputResponseDto> OutputItems { get; set; }
            = new();
    }
}