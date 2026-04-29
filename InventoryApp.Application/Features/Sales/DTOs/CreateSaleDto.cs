using InventoryApp.Domain.Enums;

namespace InventoryApp.Application.Features.Sales.DTOs
{
    // ── Each line item sent from frontend ────────────────────────
    public class CreateSaleItemDto
    {
        // Which finished shoe are we selling
        public int ItemId { get; set; }

        // How many pairs
        public decimal Quantity { get; set; }

        // Price per pair
        public decimal UnitPrice { get; set; }

        // Which price type (MRP=1, Wholesale=2, Retail=3)
        public PriceType PriceType { get; set; } = PriceType.Retail;

        // Optional discount on this line
        public decimal LineDiscount { get; set; } = 0;
    }

    // ── The full sale request ────────────────────────────────────
    public class CreateSaleDto
    {
        // Which retailer are we selling to
        public int CustomerId { get; set; }

        // Date of sale
        public DateTime SaleDate { get; set; } = DateTime.UtcNow;

        // VAT percent (0 or 13)
        public decimal VATPercent { get; set; } = 0;

        // Overall discount on the whole bill
        public decimal DiscountAmount { get; set; } = 0;

        // How much cash received now
        public decimal ReceivedAmount { get; set; } = 0;

        public string? Remarks { get; set; }

        // List of shoe items being sold
        public List<CreateSaleItemDto> Items { get; set; } = new();
    }
}