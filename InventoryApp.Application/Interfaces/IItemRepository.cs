using InventoryApp.Domain.Entities;

namespace InventoryApp.Application.Interfaces
{
    public interface IItemRepository
    {
        Task<Item> CreateAsync(Item item);
        Task<Item?> GetByIdAsync(int id);
        Task<List<Item>> GetAllAsync();
    }
}