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

        public int? ItemCategoryId { get; set; }
        public ItemCategory? Category { get; set; }

        public ICollection<ItemPrice> Prices { get; set; } = new List<ItemPrice>();
    }
}