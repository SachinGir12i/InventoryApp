namespace InventoryApp.Application.Features.Purchases.DTOs
{
    // ── Each line item sent from frontend ────────────────────────
    public class CreatePurchaseItemDto
    {
        // Which raw material are we buying
        public int ItemId { get; set; }

        // How many units
        public decimal Quantity { get; set; }

        // Price per unit agreed with supplier
        public decimal UnitPrice { get; set; }
    }

    // ── The full purchase request ────────────────────────────────
    public class CreatePurchaseDto
    {
        // Which supplier are we buying from
        public int SupplierId { get; set; }

        // Date of purchase (frontend sends this)
        public DateTime PurchaseDate { get; set; } = DateTime.UtcNow;

        // VAT percent (0 or 13 for Nepal)
        public decimal VATPercent { get; set; } = 0;

        // How much cash paid now (rest goes to credit)
        public decimal PaidAmount { get; set; } = 0;

        // Supplier's own bill number for cross reference
        public string? SupplierInvoiceNumber { get; set; }

        public string? Remarks { get; set; }

        // List of items being purchased
        public List<CreatePurchaseItemDto> Items { get; set; } = new();
    }
}