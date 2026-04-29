using InventoryApp.Domain.Entities;

namespace InventoryApp.Application.Interfaces
{
    public interface IPurchaseRepository
    {
        // Create a new purchase and update stock
        Task<Purchase> CreateAsync(Purchase purchase);

        // Get single purchase with all its line items
        Task<Purchase?> GetByIdAsync(int id);

        // Get all purchases, newest first
        Task<List<Purchase>> GetAllAsync();

        // Get purchases from a specific supplier
        Task<List<Purchase>> GetBySupplierIdAsync(int supplierId);

        // Get next purchase number e.g. PUR-0001, PUR-0002
        Task<string> GeneratePurchaseNumberAsync();
    }
}