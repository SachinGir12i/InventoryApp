using InventoryApp.Application.Interfaces;
using InventoryApp.Domain.Entities;
using InventoryApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace InventoryApp.Infrastructure.Repositories
{
    public class ProductionRepository : IProductionRepository
    {
        private readonly AppDbContext _context;
        public ProductionRepository(AppDbContext context) => _context = context;

        public async Task<Production> CreateAsync(Production production)
        {
            // ── Transaction: all steps succeed or all roll back ───
            using var transaction = await _context.Database
                .BeginTransactionAsync();

            try
            {
                // Step 1: Save the production record
                _context.Productions.Add(production);
                await _context.SaveChangesAsync();

                // Step 2: Decrease raw material stock
                // We used these materials to make shoes
                foreach (var material in production.MaterialsUsed)
                {
                    var rawMaterial = await _context.Items
                        .FindAsync(material.ItemId);

                    if (rawMaterial != null)
                    {
                        // Raw material stock goes DOWN
                        rawMaterial.CurrentStock -=
                            (int)material.QuantityUsed;
                        rawMaterial.UpdatedAt = DateTime.UtcNow;
                    }
                }

                // Step 3: Increase finished goods stock
                // We produced these shoes
                foreach (var output in production.OutputItems)
                {
                    var finishedGood = await _context.Items
                        .FindAsync(output.ItemId);

                    if (finishedGood != null)
                    {
                        // Finished shoe stock goes UP
                        finishedGood.CurrentStock +=
                            (int)output.QuantityProduced;
                        finishedGood.UpdatedAt = DateTime.UtcNow;
                    }
                }

                // Step 4: Save all stock changes
                await _context.SaveChangesAsync();

                // Step 5: All good — commit
                await transaction.CommitAsync();

                return production;
            }
            catch
            {
                // Something failed — undo everything
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<Production?> GetByIdAsync(int id)
        {
            return await _context.Productions
                // Load raw materials with their item names
                .Include(p => p.MaterialsUsed)
                    .ThenInclude(m => m.Item)
                // Load output items with their item names
                .Include(p => p.OutputItems)
                    .ThenInclude(o => o.Item)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<Production>> GetAllAsync()
        {
            return await _context.Productions
                .Include(p => p.MaterialsUsed)
                    .ThenInclude(m => m.Item)
                .Include(p => p.OutputItems)
                    .ThenInclude(o => o.Item)
                // Newest productions first
                .OrderByDescending(p => p.ProductionDate)
                .ToListAsync();
        }

        public async Task<string> GenerateProductionNumberAsync()
        {
            // e.g. PRD-0001, PRD-0002...
            var count = await _context.Productions.CountAsync();
            return $"PRD-{(count + 1):D4}";
        }
        // Get all productions in a date range for P&L
        public async Task<List<Production>> GetByDateRangeAsync(
            DateTime from, DateTime to)
        {
            return await _context.Productions
                .Where(p => p.ProductionDate >= from &&
                       p.ProductionDate <= to.AddDays(1))
                .OrderBy(p => p.ProductionDate)
                .ToListAsync();
        }
    }
}