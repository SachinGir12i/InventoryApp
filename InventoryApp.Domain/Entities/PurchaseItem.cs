using InventoryApp.Domain.Common;

namespace InventoryApp.Domain.Entities
{
    public class PurchaseItem : BaseEntity
    {
        // ── Which Purchase this belongs to ───────────────────────
        public int PurchaseId { get; set; }
        public Purchase Purchase { get; set; } = null!;

        // ── Which Item was bought ────────────────────────────────
        // e.g. Leather, Rubber Sole, Laces
        public int ItemId { get; set; }
        public Item Item { get; set; } = null!;

        // ── Quantity & Price ─────────────────────────────────────

        // How many units were bought
        public decimal Quantity { get; set; }

        // Price per unit at time of purchase
        // (stored here because price may change in future)
        public decimal UnitPrice { get; set; }

        // Total for this line = Quantity x UnitPrice
        public decimal LineTotal { get; set; }
    }
}