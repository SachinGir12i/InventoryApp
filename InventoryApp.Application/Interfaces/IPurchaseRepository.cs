using InventoryApp.Domain.Entities;

namespace InventoryApp.Application.Interfaces
{
    public interface IPurchaseRepository
    {
        Task<Purchase> CreateAsync(Purchase purchase);
        Task<Purchase?> GetByIdAsync(int id);
        Task<List<Purchase>> GetAllAsync();
        Task<List<Purchase>> GetBySupplierIdAsync(int supplierId);
        Task<string> GeneratePurchaseNumberAsync();
        Task<List<Purchase>> GetBySupplierAndDateRangeAsync(
            int supplierId, DateTime from, DateTime to);

        // ── New for P&L ──────────────────────────────────────────
        // Get all purchases within a date range
        Task<List<Purchase>> GetByDateRangeAsync(
            DateTime from, DateTime to);
    }
}