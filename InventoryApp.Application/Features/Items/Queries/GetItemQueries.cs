using InventoryApp.Application.Features.Items.DTOs;
using InventoryApp.Application.Interfaces;
using MediatR;

namespace InventoryApp.Application.Features.Items.Queries
{
    // ─── Get All Items ───────────────────────────────────────

    // 1. The Query
    public class GetAllItemsQuery : IRequest<List<ItemResponseDto>> { }

    // 2. The Handler
    public class GetAllItemsQueryHandler : IRequestHandler<GetAllItemsQuery, List<ItemResponseDto>>
    {
        private readonly IItemRepository _itemRepository;

        public GetAllItemsQueryHandler(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        public async Task<List<ItemResponseDto>> Handle(GetAllItemsQuery request, CancellationToken cancellationToken)
        {
            var items = await _itemRepository.GetAllAsync();

            return items.Select(item => new ItemResponseDto
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
                SKU = item.SKU,
                CostPrice = item.CostPrice,
                SellingPrice = item.SellingPrice,
                CurrentStock = item.CurrentStock,
                LowStockThreshold = item.LowStockThreshold,
                IsActive = item.IsActive,
                CreatedAt = item.CreatedAt
            }).ToList();
        }
    }

    // ─── Get Item By Id ──────────────────────────────────────

    // 1. The Query
    public class GetItemByIdQuery : IRequest<ItemResponseDto?>
    {
        public int Id { get; set; }
    }

    // 2. The Handler
    public class GetItemByIdQueryHandler : IRequestHandler<GetItemByIdQuery, ItemResponseDto?>
    {
        private readonly IItemRepository _itemRepository;

        public GetItemByIdQueryHandler(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        public async Task<ItemResponseDto?> Handle(GetItemByIdQuery request, CancellationToken cancellationToken)
        {
            var item = await _itemRepository.GetByIdAsync(request.Id);

            if (item == null) return null;

            return new ItemResponseDto
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
                SKU = item.SKU,
                CostPrice = item.CostPrice,
                SellingPrice = item.SellingPrice,
                CurrentStock = item.CurrentStock,
                LowStockThreshold = item.LowStockThreshold,
                IsActive = item.IsActive,
                CreatedAt = item.CreatedAt
            };
        }
    }
}