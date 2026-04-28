using InventoryApp.Application.Interfaces;
using InventoryApp.Domain.Entities;
using InventoryApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace InventoryApp.Infrastructure.Repositories
{
    public class ItemCategoryRepository : IItemCategoryRepository
    {
        private readonly AppDbContext _context;
        public ItemCategoryRepository(AppDbContext context) => _context = context;

        public async Task<ItemCategory> CreateAsync(ItemCategory category)
        {
            _context.ItemCategories.Add(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<ItemCategory?> GetByIdAsync(int id)
        {
            return await _context.ItemCategories
                .Include(c => c.Items.Where(i => i.IsActive))
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<List<ItemCategory>> GetAllAsync()
        {
            return await _context.ItemCategories
                .Include(c => c.Items.Where(i => i.IsActive))
                .Where(c => c.IsActive)
                .OrderBy(c => c.Name)
                .ToListAsync();
        }

        public async Task<ItemCategory> UpdateAsync(ItemCategory category)
        {
            category.UpdatedAt = DateTime.UtcNow;
            _context.ItemCategories.Update(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var category = await _context.ItemCategories.FindAsync(id);
            if (category == null) return false;

            category.IsActive = false;  // soft delete
            category.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}