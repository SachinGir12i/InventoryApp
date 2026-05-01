using InventoryApp.Domain.Common;

namespace InventoryApp.Domain.Entities
{
    public class Production : BaseEntity
    {
        // ── Production Identity ──────────────────────────────────

        // Auto generated e.g. PRD-0001
        public string ProductionNumber { get; set; } = string.Empty;

        // Date production happened
        public DateTime ProductionDate { get; set; } = DateTime.UtcNow;

        // Description of this production batch
        public string? Description { get; set; }

        // ── Cost Info ────────────────────────────────────────────

        // Cost of raw materials used (calculated automatically)
        public decimal RawMaterialCost { get; set; } = 0;

        // Labor cost for this batch (wages paid to workers)
        public decimal LaborCost { get; set; } = 0;

        // Any other costs (electricity, tools etc)
        public decimal OtherCost { get; set; } = 0;

        // Total production cost = materials + labor + other
        public decimal TotalCost { get; set; } = 0;

        // Total pairs produced in this batch
        public decimal TotalQuantityProduced { get; set; } = 0;

        // Cost per pair = TotalCost / TotalQuantityProduced
        public decimal CostPerUnit { get; set; } = 0;

        public string? Remarks { get; set; }

        // ── Navigation ───────────────────────────────────────────

        // Raw materials consumed in this production
        public ICollection<ProductionMaterial> MaterialsUsed { get; set; }
            = new List<ProductionMaterial>();

        // Finished shoes produced in this production
        public ICollection<ProductionOutput> OutputItems { get; set; }
            = new List<ProductionOutput>();
    }
}