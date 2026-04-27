using InventoryApp.Domain.Enums;

namespace InventoryApp.Application.Features.Parties.DTOs
{
    public class CreatePartyDto
    {
        public string Name { get; set; } = string.Empty;
        public PartyType Type { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? PANNumber { get; set; }
        public string? VATNumber { get; set; }
        public decimal OpeningBalance { get; set; } = 0;
        public decimal CreditLimit { get; set; } = 0;
        public bool CreditReminderEnabled { get; set; } = false;
        public int? ReminderDaysInterval { get; set; }
        public int? PartyCategoryId { get; set; }
        public string? Remarks { get; set; }
    }
}