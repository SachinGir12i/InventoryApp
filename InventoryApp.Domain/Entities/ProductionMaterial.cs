using InventoryApp.Domain.Common;

namespace InventoryApp.Domain.Entities
{
    // Represents a raw material consumed during production
    public class ProductionMaterial : BaseEntity
    {
        // ── Which Production this belongs to ─────────────────────
        public int ProductionId { get; set; }
        public Production Production { get; set; } = null!;

        // ── Which Raw Material was used ──────────────────────────
        // e.g. Leather, Rubber Sole, Laces
        public int ItemId { get; set; }
        public Item Item { get; set; } = null!;

        // ── Consumption Details ──────────────────────────────────

        // How much was used
        public decimal QuantityUsed { get; set; }

        // Cost per unit at time of production
        // (taken from item's last purchase price)
        public decimal UnitCost { get; set; }

        // Total cost for this material = QuantityUsed x UnitCost
        public decimal TotalCost { get; set; }
    }
}