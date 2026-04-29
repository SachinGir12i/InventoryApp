using InventoryApp.Domain.Common;
using InventoryApp.Domain.Enums;
using System.Net.ServerSentEvents;

namespace InventoryApp.Domain.Entities
{
    public class Sale : BaseEntity
    {
        // ── Bill Info ────────────────────────────────────────────

        // Auto generated bill number e.g. SAL-0001
        public string SaleNumber { get; set; } = string.Empty;

        // Date of sale
        public DateTime SaleDate { get; set; } = DateTime.UtcNow;

        // Retailer/Customer we sold to
        public int CustomerId { get; set; }
        public Party Customer { get; set; } = null!;

        // ── Financial Info ───────────────────────────────────────

        // Total before VAT and discount
        public decimal SubTotal { get; set; }

        // Discount amount given to retailer
        public decimal DiscountAmount { get; set; } = 0;

        // VAT percentage (13% in Nepal)
        public decimal VATPercent { get; set; } = 0;

        // VAT amount calculated
        public decimal VATAmount { get; set; } = 0;

        // Grand total = SubTotal - Discount + VAT
        public decimal TotalAmount { get; set; }

        // How much retailer paid immediately
        public decimal ReceivedAmount { get; set; } = 0;

        // Remaining balance = TotalAmount - ReceivedAmount
        public decimal DueAmount { get; set; } = 0;

        // ── Payment Status ───────────────────────────────────────

        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Unpaid;

        // ── Extra Info ───────────────────────────────────────────

        public string? Remarks { get; set; }

        // ── Navigation ───────────────────────────────────────────

        // Line items of this sale (each shoe sold)
        public ICollection<SaleItem> SaleItems { get; set; } = new List<SaleItem>();
    }
}