using InventoryApp.Domain.Common;

namespace InventoryApp.Domain.Entities
{
    public class Item : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? SKU { get; set; }          // unique product code
        public decimal CostPrice { get; set; }
        public decimal SellingPrice { get; set; }
        public int CurrentStock { get; set; } = 0;
        public int LowStockThreshold { get; set; } = 5;
        public bool IsActive { get; set; } = true;

        // Foreign key to the item's category (nullable)
        public int? ItemCategoryId { get; set; }

        // Navigation property to the item's category (nullable)
        public ItemCategory? Category { get; set; }

        // Collection of price records associated with this item
        public ICollection<ItemPrice> Prices { get; set; } = new List<ItemPrice>();
    }
}