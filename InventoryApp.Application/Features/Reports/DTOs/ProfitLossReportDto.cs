namespace InventoryApp.Application.Features.Reports.DTOs
{
    // ── Income section ───────────────────────────────────────────
    public class IncomeDto
    {
        // Total amount from all sales invoices
        public decimal TotalSalesRevenue { get; set; }

        // Total discount given to retailers
        // (reduces actual income)
        public decimal TotalDiscountsGiven { get; set; }

        // Total VAT collected from customers on sales
        // (this goes to government, not your profit)
        public decimal TotalVATCollected { get; set; }

        // Actual revenue after discount but before VAT
        // = TotalSalesRevenue - TotalDiscountsGiven
        public decimal NetRevenue { get; set; }
    }

    // ── Cost of Goods Sold section ───────────────────────────────
    public class COGSDto
    {
        // Total spent on raw materials (from purchases)
        public decimal TotalRawMaterialCost { get; set; }

        // Total labor cost from all production batches
        public decimal TotalLaborCost { get; set; }

        // Total other production costs
        public decimal TotalOtherProductionCost { get; set; }

        // Total COGS = materials + labor + other
        public decimal TotalCOGS { get; set; }
    }

    // ── Sales breakdown by item ──────────────────────────────────
    public class SalesBreakdownDto
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; } = string.Empty;

        // Total pairs sold
        public decimal QuantitySold { get; set; }

        // Total revenue from this item
        public decimal TotalRevenue { get; set; }

        // Average selling price
        public decimal AverageSellingPrice { get; set; }
    }

    // ── Monthly breakdown ────────────────────────────────────────
    public class MonthlyBreakdownDto
    {
        public int Month { get; set; }
        public string MonthName { get; set; } = string.Empty;
        public int Year { get; set; }

        public decimal Revenue { get; set; }
        public decimal COGS { get; set; }
        public decimal GrossProfit { get; set; }
    }

    // ── Full P&L report ──────────────────────────────────────────
    public class ProfitLossReportDto
    {
        // Report period
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;

        // ── Income ───────────────────────────────────────────────
        public IncomeDto Income { get; set; } = new();

        // ── Cost of Goods Sold ───────────────────────────────────
        public COGSDto COGS { get; set; } = new();

        // ── Profit calculations ──────────────────────────────────

        // Gross Profit = Net Revenue - Total COGS
        public decimal GrossProfit { get; set; }

        // Gross profit as percentage of revenue
        public decimal GrossProfitMargin { get; set; }

        // Net Profit = Gross Profit - Other Expenses
        public decimal NetProfit { get; set; }

        // Net profit as percentage of revenue
        public decimal NetProfitMargin { get; set; }

        // Is the business profitable this period?
        public bool IsProfitable { get; set; }

        // ── Breakdowns ───────────────────────────────────────────

        // Which shoes sold the most revenue
        public List<SalesBreakdownDto> TopSellingItems { get; set; }
            = new();

        // Month by month breakdown
        public List<MonthlyBreakdownDto> MonthlyBreakdown { get; set; }
            = new();

        // ── Quick stats ──────────────────────────────────────────
        public int TotalSalesCount { get; set; }
        public int TotalPurchasesCount { get; set; }
        public int TotalProductionBatches { get; set; }
    }
}