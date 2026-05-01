using InventoryApp.Application.Features.Reports.DTOs;
using InventoryApp.Application.Interfaces;
using MediatR;

namespace InventoryApp.Application.Features.Reports.Queries
{
    // ── The Query ────────────────────────────────────────────────
    public class GetProfitLossReportQuery : IRequest<ProfitLossReportDto>
    {
        // Default to current month
        public DateTime FromDate { get; set; }
            = new DateTime(DateTime.UtcNow.Year,
                DateTime.UtcNow.Month, 1);

        public DateTime ToDate { get; set; }
            = DateTime.UtcNow;
    }

    // ── The Handler ──────────────────────────────────────────────
    public class GetProfitLossReportQueryHandler
        : IRequestHandler<GetProfitLossReportQuery, ProfitLossReportDto>
    {
        private readonly ISaleRepository _saleRepo;
        private readonly IPurchaseRepository _purchaseRepo;
        private readonly IProductionRepository _productionRepo;

        public GetProfitLossReportQueryHandler(
            ISaleRepository saleRepo,
            IPurchaseRepository purchaseRepo,
            IProductionRepository productionRepo)
        {
            _saleRepo = saleRepo;
            _purchaseRepo = purchaseRepo;
            _productionRepo = productionRepo;
        }

        public async Task<ProfitLossReportDto> Handle(
            GetProfitLossReportQuery request,
            CancellationToken cancellationToken)
        {
            // ── Step 1: Fetch all data in date range ──────────────
            var sales = await _saleRepo
                .GetByDateRangeAsync(request.FromDate, request.ToDate);

            var purchases = await _purchaseRepo
                .GetByDateRangeAsync(request.FromDate, request.ToDate);

            var productions = await _productionRepo
                .GetByDateRangeAsync(request.FromDate, request.ToDate);

            // ── Step 2: Calculate Income ──────────────────────────

            // Total revenue from all sales
            decimal totalSalesRevenue = sales.Sum(s => s.TotalAmount);

            // Total discounts given to retailers
            decimal totalDiscounts = sales.Sum(s => s.DiscountAmount);

            // Total VAT collected
            decimal totalVATCollected = sales.Sum(s => s.VATAmount);

            // Net revenue = total sales - discounts
            // (VAT is not your income — it goes to government)
            decimal netRevenue = sales
                .Sum(s => s.TotalAmount - s.VATAmount);

            var income = new IncomeDto
            {
                TotalSalesRevenue = totalSalesRevenue,
                TotalDiscountsGiven = totalDiscounts,
                TotalVATCollected = totalVATCollected,
                NetRevenue = netRevenue
            };

            // ── Step 3: Calculate COGS ────────────────────────────

            // Raw material cost = total of all purchases
            decimal totalRawMaterialCost = purchases
                .Sum(p => p.SubTotal);

            // Labor cost = from all production batches
            decimal totalLaborCost = productions
                .Sum(p => p.LaborCost);

            // Other production costs
            decimal totalOtherCost = productions
                .Sum(p => p.OtherCost);

            // Total COGS
            decimal totalCOGS = totalRawMaterialCost
                + totalLaborCost
                + totalOtherCost;

            var cogs = new COGSDto
            {
                TotalRawMaterialCost = totalRawMaterialCost,
                TotalLaborCost = totalLaborCost,
                TotalOtherProductionCost = totalOtherCost,
                TotalCOGS = totalCOGS
            };

            // ── Step 4: Calculate Gross Profit ────────────────────

            decimal grossProfit = netRevenue - totalCOGS;

            // Gross profit margin as percentage
            // e.g. if revenue is 100 and gross profit is 45
            // then margin = 45%
            decimal grossProfitMargin = netRevenue > 0
                ? Math.Round((grossProfit / netRevenue) * 100, 2)
                : 0;

            // ── Step 5: Calculate Net Profit ──────────────────────
            // For now net profit = gross profit
            // (discounts already deducted from revenue)
            decimal netProfit = grossProfit;

            decimal netProfitMargin = netRevenue > 0
                ? Math.Round((netProfit / netRevenue) * 100, 2)
                : 0;

            // ── Step 6: Build top selling items breakdown ─────────
            var topSellingItems = sales
                // Flatten all sale items from all sales
                .SelectMany(s => s.SaleItems)
                .GroupBy(si => new
                {
                    si.ItemId,
                    ItemName = si.Item?.Name ?? "Unknown"
                })
                .Select(g => new SalesBreakdownDto
                {
                    ItemId = g.Key.ItemId,
                    ItemName = g.Key.ItemName,
                    QuantitySold = g.Sum(si => si.Quantity),
                    TotalRevenue = g.Sum(si => si.LineTotal),
                    AverageSellingPrice = g.Average(si => si.UnitPrice)
                })
                // Sort by revenue — highest first
                .OrderByDescending(s => s.TotalRevenue)
                .Take(10)    // top 10 items
                .ToList();

            // ── Step 7: Build monthly breakdown ───────────────────
            // Useful for seeing which months were most profitable
            var monthlyBreakdown = sales
                .GroupBy(s => new
                {
                    s.SaleDate.Year,
                    s.SaleDate.Month
                })
                .Select(g =>
                {
                    // Get purchases in this same month
                    var monthPurchases = purchases
                        .Where(p =>
                            p.PurchaseDate.Year == g.Key.Year &&
                            p.PurchaseDate.Month == g.Key.Month);

                    // Get productions in this same month
                    var monthProductions = productions
                        .Where(p =>
                            p.ProductionDate.Year == g.Key.Year &&
                            p.ProductionDate.Month == g.Key.Month);

                    decimal monthRevenue = g
                        .Sum(s => s.TotalAmount - s.VATAmount);

                    decimal monthCOGS =
                        monthPurchases.Sum(p => p.SubTotal) +
                        monthProductions.Sum(p => p.LaborCost) +
                        monthProductions.Sum(p => p.OtherCost);

                    return new MonthlyBreakdownDto
                    {
                        Year = g.Key.Year,
                        Month = g.Key.Month,
                        MonthName = new DateTime(
                            g.Key.Year, g.Key.Month, 1)
                            .ToString("MMMM yyyy"),
                        Revenue = monthRevenue,
                        COGS = monthCOGS,
                        GrossProfit = monthRevenue - monthCOGS
                    };
                })
                .OrderBy(m => m.Year)
                .ThenBy(m => m.Month)
                .ToList();

            // ── Step 8: Return full report ────────────────────────
            return new ProfitLossReportDto
            {
                FromDate = request.FromDate,
                ToDate = request.ToDate,
                GeneratedAt = DateTime.UtcNow,

                Income = income,
                COGS = cogs,

                GrossProfit = grossProfit,
                GrossProfitMargin = grossProfitMargin,
                NetProfit = netProfit,
                NetProfitMargin = netProfitMargin,

                // Is the business profitable?
                IsProfitable = netProfit > 0,

                TopSellingItems = topSellingItems,
                MonthlyBreakdown = monthlyBreakdown,

                TotalSalesCount = sales.Count,
                TotalPurchasesCount = purchases.Count,
                TotalProductionBatches = productions.Count
            };
        }
    }
}