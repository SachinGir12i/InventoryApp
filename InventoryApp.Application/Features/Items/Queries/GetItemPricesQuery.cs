using InventoryApp.Application.Features.Items.DTOs;
using InventoryApp.Application.Interfaces;
using MediatR;

namespace InventoryApp.Application.Features.Items.Queries
{
    public class GetItemPricesQuery : IRequest<List<ItemPriceResponseDto>>
    {
        public int ItemId { get; set; }
    }

    public class GetItemPricesQueryHandler : IRequestHandler<GetItemPricesQuery, List<ItemPriceResponseDto>>
    {
        private readonly IItemPriceRepository _repo;
        public GetItemPricesQueryHandler(IItemPriceRepository repo) => _repo = repo;

        public async Task<List<ItemPriceResponseDto>> Handle(
            GetItemPricesQuery request, CancellationToken cancellationToken)
        {
            var prices = await _repo.GetPricesByItemIdAsync(request.ItemId);

            return prices.Select(p => new ItemPriceResponseDto
            {
                Id = p.Id,
                ItemId = p.ItemId,
                ItemName = p.Item?.Name ?? string.Empty,
                PriceType = p.PriceType.ToString(),
                Price = p.Price,
                IsDefault = p.IsDefault
            }).ToList();
        }
    }
}