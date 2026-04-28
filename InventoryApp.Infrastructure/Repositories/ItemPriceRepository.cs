using InventoryApp.Application.Interfaces;
using InventoryApp.Domain.Entities;
using InventoryApp.Domain.Enums;
using InventoryApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace InventoryApp.Infrastructure.Repositories
{
    public class ItemPriceRepository : IItemPriceRepository
    {
        private readonly AppDbContext _context;
        public ItemPriceRepository(AppDbContext context) => _context = context;

        public async Task<ItemPrice> AddPriceAsync(ItemPrice price)
        {
            // if new price is default, remove default from all other prices of this item
            if (price.IsDefault)
            {
                var existingPrices = await _context.ItemPrices
                    .Where(p => p.ItemId == price.ItemId && p.IsDefault)
                    .ToListAsync();

                existingPrices.ForEach(p => p.IsDefault = false);
            }

            _context.ItemPrices.Add(price);
            await _context.SaveChangesAsync();
            return price;
        }

        public async Task<List<ItemPrice>> GetPricesByItemIdAsync(int itemId)
        {
            return await _context.ItemPrices
                .Include(p => p.Item)
                .Where(p => p.ItemId == itemId && p.IsActive)
                .ToListAsync();
        }

        public async Task<ItemPrice?> GetByItemAndTypeAsync(int itemId, PriceType priceType)
        {
            return await _context.ItemPrices
                .FirstOrDefaultAsync(p => p.ItemId == itemId &&
                                          p.PriceType == priceType &&
                                          p.IsActive);
        }

        public async Task<ItemPrice> UpdateAsync(ItemPrice price)
        {
            price.UpdatedAt = DateTime.UtcNow;
            _context.ItemPrices.Update(price);
            await _context.SaveChangesAsync();
            return price;
        }
    }
}