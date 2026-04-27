using InventoryApp.Domain.Common;
using InventoryApp.Domain.Enums;

namespace InventoryApp.Domain.Entities
{
    public class Party : BaseEntity
    {
        // Basic Info
        public string Name { get; set; } = string.Empty;
        public PartyType Type { get; set; }             // Customer, Supplier, Both

        // Contact Info
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }

        // Business Info (Nepal specific)
        public string? PANNumber { get; set; }          // PAN number for VAT billing
        public string? VATNumber { get; set; }

        // Financial Info
        public decimal OpeningBalance { get; set; } = 0; // balance before using this system
        public decimal CurrentBalance { get; set; } = 0; // running balance (+ = they owe us, - = we owe them)
        public decimal CreditLimit { get; set; } = 0;    // max credit allowed

        // Credit Reminder (Karobar feature)
        public bool CreditReminderEnabled { get; set; } = false;
        public int? ReminderDaysInterval { get; set; }   // remind every X days

        // Grouping
        public int? PartyCategoryId { get; set; }
        public PartyCategory? Category { get; set; }

        // Status
        public bool IsActive { get; set; } = true;
        public string? Remarks { get; set; }
    }
}