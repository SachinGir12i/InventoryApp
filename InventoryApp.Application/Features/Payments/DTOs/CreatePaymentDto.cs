using InventoryApp.Domain.Enums;

namespace InventoryApp.Application.Features.Payments.DTOs
{
    public class CreatePaymentDto
    {
        // Which party is this payment with
        public int PartyId { get; set; }

        // Received = retailer paying us, Paid = us paying supplier
        public PaymentType PaymentType { get; set; }

        // Date of payment
        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;

        // Amount being paid
        public decimal Amount { get; set; }

        // How payment was made
        public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.Cash;

        // Optional: which sale this is for
        public int? SaleId { get; set; }

        // Optional: which purchase this is for
        public int? PurchaseId { get; set; }

        // Bank reference number if applicable
        public string? TransactionReference { get; set; }

        public string? Remarks { get; set; }
    }
}