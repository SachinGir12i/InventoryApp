using InventoryApp.Application.Interfaces;
using InventoryApp.Domain.Entities;
using InventoryApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace InventoryApp.Infrastructure.Repositories
{
    public class PurchaseRepository : IPurchaseRepository
    {
        private readonly AppDbContext _context;

        public PurchaseRepository(AppDbContext context) => _context = context;

        public async Task<Purchase> CreateAsync(Purchase purchase)
        {
            // ── Use a transaction so either everything saves or nothing does ──
            // If stock update fails, the purchase also gets rolled back
            using var transaction = await _context.Database
                .BeginTransactionAsync();

            try
            {
                // Step 1: Save the purchase with all its line items
                _context.Purchases.Add(purchase);
                await _context.SaveChangesAsync();

                // Step 2: Update stock for each item bought
                // When we buy raw materials, stock goes UP
                foreach (var purchaseItem in purchase.PurchaseItems)
                {
                    var item = await _context.Items
                        .FindAsync(purchaseItem.ItemId);

                    if (item != null)
                    {
                        // Increase stock by quantity purchased
                        item.CurrentStock += (int)purchaseItem.Quantity;
                        item.UpdatedAt = DateTime.UtcNow;
                    }
                }

                // Step 3: Update supplier's balance
                // We owe the supplier the due amount
                var supplier = await _context.Parties
                    .FindAsync(purchase.SupplierId);

                if (supplier != null)
                {
                    // Negative balance = we owe them money
                    supplier.CurrentBalance -= purchase.DueAmount;
                    supplier.UpdatedAt = DateTime.UtcNow;
                }

                // Step 4: Save stock and balance changes
                await _context.SaveChangesAsync();

                // Step 5: All good — commit the transaction
                await transaction.CommitAsync();

                return purchase;
            }
            catch
            {
                // Something went wrong — roll back everything
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<Purchase?> GetByIdAsync(int id)
        {
            return await _context.Purchases
                // Load supplier info
                .Include(p => p.Supplier)
                // Load line items
                .Include(p => p.PurchaseItems)
                    // Load item name for each line item
                    .ThenInclude(pi => pi.Item)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<Purchase>> GetAllAsync()
        {
            return await _context.Purchases
                .Include(p => p.Supplier)
                .Include(p => p.PurchaseItems)
                    .ThenInclude(pi => pi.Item)
                // Newest purchases first
                .OrderByDescending(p => p.PurchaseDate)
                .ToListAsync();
        }

        public async Task<List<Purchase>> GetBySupplierIdAsync(int supplierId)
        {
            return await _context.Purchases
                .Include(p => p.Supplier)
                .Include(p => p.PurchaseItems)
                    .ThenInclude(pi => pi.Item)
                .Where(p => p.SupplierId == supplierId)
                .OrderByDescending(p => p.PurchaseDate)
                .ToListAsync();
        }

        public async Task<string> GeneratePurchaseNumberAsync()
        {
            // Count existing purchases to generate next number
            // e.g. if 5 purchases exist, next is PUR-0006
            var count = await _context.Purchases.CountAsync();
            return $"PUR-{(count + 1):D4}";
        }
    }
}