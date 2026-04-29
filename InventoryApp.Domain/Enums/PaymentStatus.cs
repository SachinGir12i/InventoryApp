namespace InventoryApp.Domain.Enums
{
    public enum PaymentStatus
    {
        Unpaid = 1,       // nothing paid yet
        PartiallyPaid = 2, // some amount paid
        FullyPaid = 3      // completely settled
    }
}