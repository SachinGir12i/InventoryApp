namespace InventoryApp.Application.Features.Purchases.DTOs
{
    // ── Line item in response ────────────────────────────────────
    public class PurchaseItemResponseDto
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal LineTotal { get; set; }
    }

    // ── Full purchase response ───────────────────────────────────
    public class PurchaseResponseDto
    {
        public int Id { get; set; }
        public string PurchaseNumber { get; set; } = string.Empty;
        public DateTime PurchaseDate { get; set; }

        // Supplier info
        public int SupplierId { get; set; }
        public string SupplierName { get; set; } = string.Empty;

        // Financial breakdown
        public decimal SubTotal { get; set; }
        public decimal VATPercent { get; set; }
        public decimal VATAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal DueAmount { get; set; }

        public string PaymentStatus { get; set; } = string.Empty;
        public string? SupplierInvoiceNumber { get; set; }
        public string? Remarks { get; set; }
        public DateTime CreatedAt { get; set; }

        // All line items
        public List<PurchaseItemResponseDto> Items { get; set; } = new();
    }
}