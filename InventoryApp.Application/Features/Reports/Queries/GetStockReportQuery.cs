using InventoryApp.Application.Features.Reports.DTOs;
using InventoryApp.Application.Interfaces;
using MediatR;

namespace InventoryApp.Application.Features.Reports.Queries
{
    // ── The Query ────────────────────────────────────────────────
    public class GetStockReportQuery : IRequest<StockReportDto>
    {
        // Optional: filter by category id
        // If null, returns all items
        public int? CategoryId { get; set; }
    }

    // ── The Handler ──────────────────────────────────────────────
    public class GetStockReportQueryHandler
        : IRequestHandler<GetStockReportQuery, StockReportDto>
    {
        private readonly IItemRepository _itemRepo;
        private readonly IItemCategoryRepository _categoryRepo;

        public GetStockReportQueryHandler(
            IItemRepository itemRepo,
            IItemCategoryRepository categoryRepo)
        {
            _itemRepo = itemRepo;
            _categoryRepo = categoryRepo;
        }

        public async Task<StockReportDto> Handle(
            GetStockReportQuery request,
            CancellationToken cancellationToken)
        {
            // ── Step 1: Get all items ─────────────────────────────
            var allItems = request.CategoryId.HasValue
                // Filter by category if specified
                ? await _itemRepo.GetByCategoryAsync(request.CategoryId.Value)
                // Otherwise get everything
                : await _itemRepo.GetAllAsync();

            // ── Step 2: Map items to StockItemDto ────────────────
            var stockItems = allItems.Select(item => new StockItemDto
            {
                Id = item.Id,
                Name = item.Name,
                CategoryName = item.Category?.Name ?? "Uncategorized",
                CurrentStock = item.CurrentStock,
                LowStockThreshold = item.LowStockThreshold,
                CostPrice = item.CostPrice,
                SellingPrice = item.SellingPrice,

                // Stock value = how much the current stock is worth
                StockValue = item.CurrentStock * item.CostPrice,

                // Determine stock status
                StockStatus = item.CurrentStock <= 0
                    ? "Out of Stock"
                    : item.CurrentStock <= item.LowStockThreshold
                        ? "Low Stock"
                        : "OK",

                IsActive = item.IsActive

            }).ToList();

            // ── Step 3: Separate raw materials and finished goods ─
            // Raw Materials = items in "Raw Materials" category
            // Finished Goods = items in "Finished Goods" category
            var rawMaterials = stockItems
                .Where(i => i.CategoryName
                    .Contains("Raw Material",
                        StringComparison.OrdinalIgnoreCase))
                .OrderBy(i => i.Name)
                .ToList();

            var finishedGoods = stockItems
                .Where(i => i.CategoryName
                    .Contains("Finished",
                        StringComparison.OrdinalIgnoreCase))
                .OrderBy(i => i.Name)
                .ToList();

            // ── Step 4: Build alert lists ─────────────────────────

            // Items that need restocking soon
            var lowStockAlerts = stockItems
                .Where(i => i.StockStatus == "Low Stock")
                .OrderBy(i => i.CurrentStock)  // most urgent first
                .ToList();

            // Items completely out of stock
            var outOfStockAlerts = stockItems
                .Where(i => i.StockStatus == "Out of Stock")
                .OrderBy(i => i.Name)
                .ToList();

            // ── Step 5: Calculate summary numbers ────────────────
            var summary = new StockSummaryDto
            {
                TotalItems = stockItems.Count,

                InStockItems = stockItems
                    .Count(i => i.StockStatus == "OK"),

                LowStockItems = stockItems
                    .Count(i => i.StockStatus == "Low Stock"),

                OutOfStockItems = stockItems
                    .Count(i => i.StockStatus == "Out of Stock"),

                // Total value at cost price
                TotalStockValue = stockItems
                    .Sum(i => i.StockValue),

                // Total value at selling price
                TotalSellingValue = stockItems
                    .Sum(i => i.CurrentStock * i.SellingPrice),

                // Potential profit if all stock is sold
                PotentialProfit = stockItems
                    .Sum(i => i.CurrentStock *
                        (i.SellingPrice - i.CostPrice))
            };

            // ── Step 6: Build and return the full report ──────────
            return new StockReportDto
            {
                GeneratedAt = DateTime.UtcNow,
                Summary = summary,
                RawMaterials = rawMaterials,
                FinishedGoods = finishedGoods,
                LowStockAlerts = lowStockAlerts,
                OutOfStockAlerts = outOfStockAlerts
            };
        }
    }
}