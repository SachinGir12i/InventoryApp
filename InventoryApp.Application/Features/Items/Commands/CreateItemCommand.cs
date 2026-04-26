using InventoryApp.Application.Features.Items.DTOs;
using InventoryApp.Application.Interfaces;
using InventoryApp.Domain.Entities;
using MediatR;

namespace InventoryApp.Application.Features.Items.Commands
{
    // 1. The Command — what we want to do
    public class CreateItemCommand : IRequest<int>
    {
        public CreateItemDto Item { get; set; } = null!;
    }

    // 2. The Handler — how we do it
    public class CreateItemCommandHandler : IRequestHandler<CreateItemCommand, int>
    {
        private readonly IItemRepository _itemRepository;

        public CreateItemCommandHandler(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        public async Task<int> Handle(CreateItemCommand request, CancellationToken cancellationToken)
        {
            var item = new Item
            {
                Name = request.Item.Name,
                Description = request.Item.Description,
                SKU = request.Item.SKU,
                CostPrice = request.Item.CostPrice,
                SellingPrice = request.Item.SellingPrice,
                LowStockThreshold = request.Item.LowStockThreshold
            };

            var created = await _itemRepository.CreateAsync(item);
            return created.Id;
        }
    }
}