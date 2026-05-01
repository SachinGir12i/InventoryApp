using InventoryApp.Application.Features.Reports.DTOs;
using InventoryApp.Application.Interfaces;
using MediatR;

namespace InventoryApp.Application.Features.Reports.Queries
{
    // ── Quick query just for low stock alerts ────────────────────
    // Useful for showing a badge/notification on dashboard
    public class GetLowStockAlertsQuery : IRequest<List<StockItemDto>> { }

    public class GetLowStockAlertsQueryHandler
        : IRequestHandler<GetLowStockAlertsQuery, List<StockItemDto>>
    {
        private readonly IItemRepository _itemRepo;

        public GetLowStockAlertsQueryHandler(IItemRepository itemRepo)
            => _itemRepo = itemRepo;

        public async Task<List<StockItemDto>> Handle(
            GetLowStockAlertsQuery request,
            CancellationToken cancellationToken)
        {
            // Get items at or below threshold
            var lowStockItems = await _itemRepo.GetLowStockItemsAsync();

            return lowStockItems.Select(item => new StockItemDto
            {
                Id = item.Id,
                Name = item.Name,
                CategoryName = item.Category?.Name ?? "Uncategorized",
                CurrentStock = item.CurrentStock,
                LowStockThreshold = item.LowStockThreshold,
                CostPrice = item.CostPrice,
                SellingPrice = item.SellingPrice,
                StockValue = item.CurrentStock * item.CostPrice,
                StockStatus = item.CurrentStock <= 0
                    ? "Out of Stock"
                    : "Low Stock"
            })
            // Show most urgent (lowest stock) first
            .OrderBy(i => i.CurrentStock)
            .ToList();
        }
    }
}