namespace InventoryApp.Application.Features.Reports.DTOs
{
    // ── Single item in the stock report ─────────────────────────
    public class StockItemDto
    {
        public int Id { get; set; }

        // Item name e.g. "Leather", "Shoes Size 6"
        public string Name { get; set; } = string.Empty;

        // Category e.g. "Raw Materials", "Finished Goods"
        public string CategoryName { get; set; } = string.Empty;

        // Current stock quantity
        public int CurrentStock { get; set; }

        // Threshold below which item is considered low stock
        public int LowStockThreshold { get; set; }

        // Cost price — used to calculate stock value
        public decimal CostPrice { get; set; }

        // Selling price
        public decimal SellingPrice { get; set; }

        // Total value of this item in stock
        // = CurrentStock x CostPrice
        public decimal StockValue { get; set; }

        // "OK", "Low Stock", "Out of Stock"
        public string StockStatus { get; set; } = string.Empty;

        public bool IsActive { get; set; }
    }

    // ── Summary section of the report ────────────────────────────
    public class StockSummaryDto
    {
        // Total number of active items
        public int TotalItems { get; set; }

        // Items with stock above threshold
        public int InStockItems { get; set; }

        // Items at or below threshold but not zero
        public int LowStockItems { get; set; }

        // Items with zero stock
        public int OutOfStockItems { get; set; }

        // Total value of all stock at cost price
        public decimal TotalStockValue { get; set; }

        // Total value at selling price
        // (potential revenue if everything is sold)
        public decimal TotalSellingValue { get; set; }

        // Potential profit = TotalSellingValue - TotalStockValue
        public decimal PotentialProfit { get; set; }
    }

    // ── Full stock report ────────────────────────────────────────
    public class StockReportDto
    {
        // When this report was generated
        public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;

        // Summary numbers at the top
        public StockSummaryDto Summary { get; set; } = new();

        // All raw material items
        public List<StockItemDto> RawMaterials { get; set; } = new();

        // All finished goods items
        public List<StockItemDto> FinishedGoods { get; set; } = new();

        // Items that need immediate attention
        public List<StockItemDto> LowStockAlerts { get; set; } = new();

        // Items completely out of stock
        public List<StockItemDto> OutOfStockAlerts { get; set; } = new();
    }
}