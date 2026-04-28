using InventoryApp.Domain.Entities;
using InventoryApp.Domain.Enums;

namespace InventoryApp.Application.Interfaces
{
    public interface IItemPriceRepository
    {
        Task<ItemPrice> AddPriceAsync(ItemPrice price);
        Task<List<ItemPrice>> GetPricesByItemIdAsync(int itemId);
        Task<ItemPrice?> GetByItemAndTypeAsync(int itemId, PriceType priceType);
        Task<ItemPrice> UpdateAsync(ItemPrice price);
    }
}