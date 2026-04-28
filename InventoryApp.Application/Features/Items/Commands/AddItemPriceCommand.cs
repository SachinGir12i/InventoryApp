using InventoryApp.Application.Features.Items.DTOs;
using InventoryApp.Application.Interfaces;
using InventoryApp.Domain.Entities;
using MediatR;

namespace InventoryApp.Application.Features.Items.Commands
{
    public class AddItemPriceCommand : IRequest<int>
    {
        public AddItemPriceDto ItemPrice { get; set; } = null!;
    }

    public class AddItemPriceCommandHandler : IRequestHandler<AddItemPriceCommand, int>
    {
        private readonly IItemPriceRepository _repo;
        public AddItemPriceCommandHandler(IItemPriceRepository repo) => _repo = repo;

        public async Task<int> Handle(AddItemPriceCommand request, CancellationToken cancellationToken)
        {
            // if this price is being set as default,
            // the repository will handle unsetting the previous default
            var price = new ItemPrice
            {
                ItemId = request.ItemPrice.ItemId,
                PriceType = request.ItemPrice.PriceType,
                Price = request.ItemPrice.Price,
                IsDefault = request.ItemPrice.IsDefault
            };

            var created = await _repo.AddPriceAsync(price);
            return created.Id;
        }
    }
}