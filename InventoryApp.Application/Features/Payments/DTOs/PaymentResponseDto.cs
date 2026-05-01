namespace InventoryApp.Application.Features.Payments.DTOs
{
    public class PaymentResponseDto
    {
        public int Id { get; set; }
        public string PaymentNumber { get; set; } = string.Empty;
        public DateTime PaymentDate { get; set; }

        // Party info
        public int PartyId { get; set; }
        public string PartyName { get; set; } = string.Empty;

        // Payment details
        public string PaymentType { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;

        // Reference info
        public int? SaleId { get; set; }
        public string? SaleNumber { get; set; }
        public int? PurchaseId { get; set; }
        public string? PurchaseNumber { get; set; }
        public string? TransactionReference { get; set; }
        public string? Remarks { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}