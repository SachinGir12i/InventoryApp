using InventoryApp.Domain.Common;
using InventoryApp.Domain.Enums;

namespace InventoryApp.Domain.Entities
{
    public class Payment : BaseEntity
    {
        // ── Payment Identity ─────────────────────────────────────

        // Auto generated e.g. PAY-0001
        public string PaymentNumber { get; set; } = string.Empty;

        // Date payment was made
        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;

        // ── Who is paying / being paid ───────────────────────────

        // The party involved (retailer or supplier)
        public int PartyId { get; set; }
        public Party Party { get; set; } = null!;

        // Is this money coming IN or going OUT
        public PaymentType PaymentType { get; set; }

        // ── Amount Info ──────────────────────────────────────────

        // How much was paid
        public decimal Amount { get; set; }

        // How they paid (cash, bank, eSewa etc)
        public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.Cash;

        // ── Reference Info ───────────────────────────────────────

        // Optional: link to a specific sale (SAL-0001)
        // so you know which bill this payment is for
        public int? SaleId { get; set; }
        public Sale? Sale { get; set; }

        // Optional: link to a specific purchase (PUR-0001)
        public int? PurchaseId { get; set; }
        public Purchase? Purchase { get; set; }

        // Bank transaction reference if paid by bank
        public string? TransactionReference { get; set; }

        // Any notes about this payment
        public string? Remarks { get; set; }
    }
}