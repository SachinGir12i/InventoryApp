using InventoryApp.Domain.Entities;

namespace InventoryApp.Application.Interfaces
{
    public interface IProductionRepository
    {
        Task<Production> CreateAsync(Production production);
        Task<Production?> GetByIdAsync(int id);
        Task<List<Production>> GetAllAsync();
        Task<string> GenerateProductionNumberAsync();

        // ── New for P&L ──────────────────────────────────────────
        // Get all productions within a date range
        Task<List<Production>> GetByDateRangeAsync(
            DateTime from, DateTime to);
    }
}