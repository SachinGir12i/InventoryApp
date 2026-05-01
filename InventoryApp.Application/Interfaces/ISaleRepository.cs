using InventoryApp.Domain.Entities;

namespace InventoryApp.Application.Interfaces
{
    public interface ISaleRepository
    {
        Task<Sale> CreateAsync(Sale sale);
        Task<Sale?> GetByIdAsync(int id);
        Task<List<Sale>> GetAllAsync();
        Task<List<Sale>> GetByCustomerIdAsync(int customerId);
        Task<string> GenerateSaleNumberAsync();
        Task<List<Sale>> GetByCustomerAndDateRangeAsync(
            int customerId, DateTime from, DateTime to);

        // ── New for P&L ──────────────────────────────────────────
        // Get all sales within a date range for P&L calculation
        Task<List<Sale>> GetByDateRangeAsync(
            DateTime from, DateTime to);
    }
}