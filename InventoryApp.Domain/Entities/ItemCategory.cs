using InventoryApp.Domain.Common;

namespace InventoryApp.Domain.Entities
{
    public class ItemCategory : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;

        // Navigation
        public ICollection<Item> Items { get; set; } = new List<Item>();
    }
}