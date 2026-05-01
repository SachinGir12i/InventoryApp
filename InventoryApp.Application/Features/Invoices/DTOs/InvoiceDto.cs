namespace InventoryApp.Application.Features.Invoices.DTOs
{
    // ── Company info shown at top of invoice ─────────────────────
    public class CompanyInfoDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? PANNumber { get; set; }
    }

    // ── Each line item on the invoice ────────────────────────────
    public class InvoiceLineItemDto
    {
        public int SNo { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }
        public decimal Amount { get; set; }
    }

    // ── Full invoice data ────────────────────────────────────────
    public class InvoiceDto
    {
        // Invoice header
        public string InvoiceNumber { get; set; } = string.Empty;
        public DateTime InvoiceDate { get; set; }

        // Your factory info
        public CompanyInfoDto Company { get; set; } = new();

        // Retailer info
        public string CustomerName { get; set; } = string.Empty;
        public string? CustomerAddress { get; set; }
        public string? CustomerPhone { get; set; }
        public string? CustomerPAN { get; set; }

        // Line items
        public List<InvoiceLineItemDto> LineItems { get; set; }
            = new();

        // Financial totals
        public decimal SubTotal { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal VATPercent { get; set; }
        public decimal VATAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal ReceivedAmount { get; set; }
        public decimal DueAmount { get; set; }

        // Amount in words e.g. "Twenty Two Thousand Only"
        public string AmountInWords { get; set; } = string.Empty;

        // Payment status
        public string PaymentStatus { get; set; } = string.Empty;

        public string? Remarks { get; set; }
    }
}