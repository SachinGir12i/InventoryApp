using InventoryApp.Domain.Enums;

namespace InventoryApp.Application.Features.Items.DTOs
{
    public class AddItemPriceDto
    {
        public int ItemId { get; set; }
        public PriceType PriceType { get; set; }
        public decimal Price { get; set; }
        public bool IsDefault { get; set; } = false;
    }

    public class ItemPriceResponseDto
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public string PriceType { get; set; } = string.Empty;  // "MRP", "Wholesale" etc
        public decimal Price { get; set; }
        public bool IsDefault { get; set; }
    }
}