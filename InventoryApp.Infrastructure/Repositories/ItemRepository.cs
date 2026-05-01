using InventoryApp.Application.Interfaces;
using InventoryApp.Domain.Entities;
using InventoryApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace InventoryApp.Infrastructure.Repositories
{
    public class ItemRepository : IItemRepository
    {
        private readonly AppDbContext _context;
        public ItemRepository(AppDbContext context) => _context = context;

        public async Task<Item> CreateAsync(Item item)
        {
            _context.Items.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<Item?> GetByIdAsync(int id)
        {
            return await _context.Items
                .Include(i => i.Category)
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<List<Item>> GetAllAsync()
        {
            return await _context.Items
                .Include(i => i.Category)
                .Where(i => i.IsActive)
                .OrderBy(i => i.Name)
                .ToListAsync();
        }

        // Get items at or below their low stock threshold
        public async Task<List<Item>> GetLowStockItemsAsync()
        {
            return await _context.Items
                .Include(i => i.Category)
                .Where(i => i.IsActive &&
                       i.CurrentStock <= i.LowStockThreshold &&
                       i.CurrentStock > 0)
                .OrderBy(i => i.CurrentStock)
                .ToListAsync();
        }

        // Get items with zero stock
        public async Task<List<Item>> GetOutOfStockItemsAsync()
        {
            return await _context.Items
                .Include(i => i.Category)
                .Where(i => i.IsActive && i.CurrentStock <= 0)
                .OrderBy(i => i.Name)
                .ToListAsync();
        }

        // Get items filtered by category
        public async Task<List<Item>> GetByCategoryAsync(int categoryId)
        {
            return await _context.Items
                .Include(i => i.Category)
                .Where(i => i.IsActive &&
                       i.ItemCategoryId == categoryId)
                .OrderBy(i => i.Name)
                .ToListAsync();
        }
    }
}