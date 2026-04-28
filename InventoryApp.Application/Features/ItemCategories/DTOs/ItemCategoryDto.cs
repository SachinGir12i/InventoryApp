namespace InventoryApp.Application.Features.ItemCategories.DTOs
{
    public class CreateItemCategoryDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }

    public class ItemCategoryResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int ItemCount { get; set; }    // how many items are in this category
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}