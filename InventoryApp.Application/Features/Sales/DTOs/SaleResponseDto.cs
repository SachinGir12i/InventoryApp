namespace InventoryApp.Application.Features.Sales.DTOs
{
    // ── Line item in response ────────────────────────────────────
    public class SaleItemResponseDto
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string PriceType { get; set; } = string.Empty;
        public decimal LineDiscount { get; set; }
        public decimal LineTotal { get; set; }
    }

    // ── Full sale response ───────────────────────────────────────
    public class SaleResponseDto
    {
        public int Id { get; set; }
        public string SaleNumber { get; set; } = string.Empty;
        public DateTime SaleDate { get; set; }

        // Customer/Retailer info
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;

        // Financial breakdown
        public decimal SubTotal { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal VATPercent { get; set; }
        public decimal VATAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal ReceivedAmount { get; set; }
        public decimal DueAmount { get; set; }

        public string PaymentStatus { get; set; } = string.Empty;
        public string? Remarks { get; set; }
        public DateTime CreatedAt { get; set; }

        // All line items
        public List<SaleItemResponseDto> Items { get; set; } = new();
    }
}