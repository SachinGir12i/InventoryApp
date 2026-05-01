namespace InventoryApp.Domain.Enums
{
    public enum PaymentType
    {
        // Money coming IN — retailer paying you
        Received = 1,

        // Money going OUT — you paying supplier
        Paid = 2
    }
}