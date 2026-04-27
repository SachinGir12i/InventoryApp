using InventoryApp.Domain.Enums;

namespace InventoryApp.Application.Features.Parties.DTOs
{
    public class PartyResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;   // "Customer", "Supplier", "Both"
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? PANNumber { get; set; }
        public string? VATNumber { get; set; }
        public decimal OpeningBalance { get; set; }
        public decimal CurrentBalance { get; set; }
        public decimal CreditLimit { get; set; }
        public bool CreditReminderEnabled { get; set; }
        public int? ReminderDaysInterval { get; set; }
        public string? CategoryName { get; set; }
        public bool IsActive { get; set; }
        public string? Remarks { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}