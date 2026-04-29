using InventoryApp.Domain.Common;
using InventoryApp.Domain.Enums;

namespace InventoryApp.Domain.Entities
{
    public class Purchase : BaseEntity
    {
        // ── Bill Info ────────────────────────────────────────────

        // Auto generated bill number e.g. PUR-0001
        public string PurchaseNumber { get; set; } = string.Empty;

        // Date of purchase
        public DateTime PurchaseDate { get; set; } = DateTime.UtcNow;

        // Supplier we bought from (must be a Party with type Supplier)
        public int SupplierId { get; set; }
        public Party Supplier { get; set; } = null!;

        // ── Financial Info ───────────────────────────────────────

        // Total before VAT
        public decimal SubTotal { get; set; }

        // VAT percentage applied (13% in Nepal)
        public decimal VATPercent { get; set; } = 0;

        // VAT amount calculated
        public decimal VATAmount { get; set; } = 0;

        // Grand total = SubTotal + VATAmount
        public decimal TotalAmount { get; set; }

        // How much was paid at time of purchase
        public decimal PaidAmount { get; set; } = 0;

        // Remaining balance = TotalAmount - PaidAmount
        public decimal DueAmount { get; set; } = 0;

        // ── Payment Status ───────────────────────────────────────

        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Unpaid;

        // ── Extra Info ───────────────────────────────────────────

        // Supplier's own bill/invoice number for reference
        public string? SupplierInvoiceNumber { get; set; }

        // Any extra notes about this purchase
        public string? Remarks { get; set; }

        // ── Navigation ───────────────────────────────────────────

        // Line items of this purchase (each raw material bought)
        public ICollection<PurchaseItem> PurchaseItems { get; set; } = new List<PurchaseItem>();
    }
}