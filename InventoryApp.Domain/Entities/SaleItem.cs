using InventoryApp.Domain.Common;
using InventoryApp.Domain.Enums;

namespace InventoryApp.Domain.Entities
{
    public class SaleItem : BaseEntity
    {
        // ── Which Sale this belongs to ───────────────────────────
        public int SaleId { get; set; }
        public Sale Sale { get; set; } = null!;

        // ── Which Item was sold ──────────────────────────────────
        // e.g. Shoes Size 6, Shoes Size 7
        public int ItemId { get; set; }
        public Item Item { get; set; } = null!;

        // ── Quantity & Price ─────────────────────────────────────

        // How many pairs sold
        public decimal Quantity { get; set; }

        // Selling price per unit at time of sale
        // Stored here because price may change later
        public decimal UnitPrice { get; set; }

        // Which price type was used (MRP, Wholesale, Retail)
        public PriceType PriceType { get; set; } = PriceType.Retail;

        // Discount on this specific line item
        public decimal LineDiscount { get; set; } = 0;

        // Total for this line = (Quantity x UnitPrice) - LineDiscount
        public decimal LineTotal { get; set; }
    }
}