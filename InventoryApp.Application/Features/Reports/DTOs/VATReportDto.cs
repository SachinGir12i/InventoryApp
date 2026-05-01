namespace InventoryApp.Application.Features.Reports.DTOs
{
    // ── A single VAT transaction line ────────────────────────────
    public class VATTransactionDto
    {
        // Date of the transaction
        public DateTime Date { get; set; }

        // Invoice or bill number
        public string ReferenceNumber { get; set; } = string.Empty;

        // Party name (retailer or supplier)
        public string PartyName { get; set; } = string.Empty;

        // PAN number of the party
        public string? PartyPAN { get; set; }

        // Amount before VAT
        public decimal TaxableAmount { get; set; }

        // VAT percentage applied
        public decimal VATPercent { get; set; }

        // VAT amount
        public decimal VATAmount { get; set; }

        // Total amount including VAT
        public decimal TotalAmount { get; set; }
    }

    // ── Output VAT section (VAT on Sales) ────────────────────────
    public class OutputVATDto
    {
        // All sales that had VAT applied
        public List<VATTransactionDto> Transactions { get; set; }
            = new();

        // Total taxable amount across all VAT sales
        public decimal TotalTaxableAmount { get; set; }

        // Total VAT collected from retailers
        public decimal TotalVATAmount { get; set; }

        // Total invoice value including VAT
        public decimal TotalAmount { get; set; }

        // Number of VAT invoices issued
        public int TransactionCount { get; set; }
    }

    // ── Input VAT section (VAT on Purchases) ─────────────────────
    public class InputVATDto
    {
        // All purchases that had VAT applied
        public List<VATTransactionDto> Transactions { get; set; }
            = new();

        // Total taxable amount across all VAT purchases
        public decimal TotalTaxableAmount { get; set; }

        // Total VAT paid to suppliers
        public decimal TotalVATAmount { get; set; }

        // Total bill value including VAT
        public decimal TotalAmount { get; set; }

        // Number of VAT bills received
        public int TransactionCount { get; set; }
    }

    // ── VAT Summary ──────────────────────────────────────────────
    public class VATSummaryDto
    {
        // Total VAT collected from customers
        public decimal OutputVAT { get; set; }

        // Total VAT paid to suppliers
        public decimal InputVAT { get; set; }

        // Amount to pay to IRD = Output - Input
        // If positive → you owe IRD
        // If negative → IRD owes you (rare but possible)
        public decimal VATPayable { get; set; }

        // Clear message for the owner
        public string VATStatus { get; set; } = string.Empty;
    }

    // ── Full VAT Report ──────────────────────────────────────────
    public class VATReportDto
    {
        // Report period
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;

        // VAT rate used in Nepal
        public decimal VATRate { get; set; } = 13;

        // Output VAT section (sales)
        public OutputVATDto OutputVAT { get; set; } = new();

        // Input VAT section (purchases)
        public InputVATDto InputVAT { get; set; } = new();

        // Summary — the most important part
        public VATSummaryDto Summary { get; set; } = new();
    }
}