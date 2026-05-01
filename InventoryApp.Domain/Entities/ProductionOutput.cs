using InventoryApp.Domain.Common;

namespace InventoryApp.Domain.Entities
{
    // Represents a finished shoe produced during production
    public class ProductionOutput : BaseEntity
    {
        // ── Which Production this belongs to ─────────────────────
        public int ProductionId { get; set; }
        public Production Production { get; set; } = null!;

        // ── Which Finished Product was made ─────────────────────
        // e.g. Shoes Size 6, Shoes Size 7
        public int ItemId { get; set; }
        public Item Item { get; set; } = null!;

        // ── Output Details ───────────────────────────────────────

        // How many pairs were produced
        public decimal QuantityProduced { get; set; }

        // Cost per unit for this output
        // (calculated from total production cost)
        public decimal UnitCost { get; set; }
    }
}