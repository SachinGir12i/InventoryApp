using InventoryApp.Domain.Entities;

namespace InventoryApp.Application.Interfaces
{
    public interface IItemRepository
    {
        Task<Item> CreateAsync(Item item);
        Task<Item?> GetByIdAsync(int id);
        Task<List<Item>> GetAllAsync();
        // ── New methods for stock report ─────────────────────────

        // Get all items that are at or below their low stock threshold
        Task<List<Item>> GetLowStockItemsAsync();

        // Get all items that have zero stock
        Task<List<Item>> GetOutOfStockItemsAsync();

        // Get items filtered by category name
        Task<List<Item>> GetByCategoryAsync(int categoryId);
    }
}