using InventoryApp.Domain.Common;
using InventoryApp.Domain.Enums;

namespace InventoryApp.Domain.Entities
{
    public class ItemPrice : BaseEntity
    {
        public int ItemId { get; set; }
        public Item Item { get; set; } = null!;

        public PriceType PriceType { get; set; }   // MRP, Wholesale, Retail, Special
        public decimal Price { get; set; }
        public bool IsDefault { get; set; } = false; // which price shows by default on invoice
        public bool IsActive { get; set; } = true;
    }
}