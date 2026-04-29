using InventoryApp.Application.Interfaces;
using InventoryApp.Domain.Entities;
using InventoryApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace InventoryApp.Infrastructure.Repositories
{
    public class SaleRepository : ISaleRepository
    {
        private readonly AppDbContext _context;
        public SaleRepository(AppDbContext context) => _context = context;

        public async Task<Sale> CreateAsync(Sale sale)
        {
            // ── Transaction: all steps succeed or all roll back ───
            using var transaction = await _context.Database
                .BeginTransactionAsync();

            try
            {
                // Step 1: Save the sale with all line items
                _context.Sales.Add(sale);
                await _context.SaveChangesAsync();

                // Step 2: Decrease stock for each item sold
                // When we sell finished shoes, stock goes DOWN
                foreach (var saleItem in sale.SaleItems)
                {
                    var item = await _context.Items
                        .FindAsync(saleItem.ItemId);

                    if (item != null)
                    {
                        // Decrease stock by quantity sold
                        item.CurrentStock -= (int)saleItem.Quantity;
                        item.UpdatedAt = DateTime.UtcNow;
                    }
                }

                // Step 3: Update customer balance
                // Customer owes us the due amount
                var customer = await _context.Parties
                    .FindAsync(sale.CustomerId);

                if (customer != null)
                {
                    // Positive balance = they owe us money
                    customer.CurrentBalance += sale.DueAmount;
                    customer.UpdatedAt = DateTime.UtcNow;
                }

                // Step 4: Save all changes
                await _context.SaveChangesAsync();

                // Step 5: Everything worked — commit
                await transaction.CommitAsync();

                return sale;
            }
            catch
            {
                // Something failed — undo everything
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<Sale?> GetByIdAsync(int id)
        {
            return await _context.Sales
                // Load customer info
                .Include(s => s.Customer)
                // Load line items
                .Include(s => s.SaleItems)
                    // Load item name for each line
                    .ThenInclude(si => si.Item)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<List<Sale>> GetAllAsync()
        {
            return await _context.Sales
                .Include(s => s.Customer)
                .Include(s => s.SaleItems)
                    .ThenInclude(si => si.Item)
                // Newest sales first
                .OrderByDescending(s => s.SaleDate)
                .ToListAsync();
        }

        public async Task<List<Sale>> GetByCustomerIdAsync(int customerId)
        {
            return await _context.Sales
                .Include(s => s.Customer)
                .Include(s => s.SaleItems)
                    .ThenInclude(si => si.Item)
                .Where(s => s.CustomerId == customerId)
                .OrderByDescending(s => s.SaleDate)
                .ToListAsync();
        }

        public async Task<string> GenerateSaleNumberAsync()
        {
            // e.g. SAL-0001, SAL-0002...
            var count = await _context.Sales.CountAsync();
            return $"SAL-{(count + 1):D4}";
        }
    }
}