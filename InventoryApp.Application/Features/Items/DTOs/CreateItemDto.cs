namespace InventoryApp.Application.Features.Items.DTOs
{
    public class CreateItemDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? SKU { get; set; }
        public decimal CostPrice { get; set; }
        public decimal SellingPrice { get; set; }
        public int LowStockThreshold { get; set; } = 5;
    }
}