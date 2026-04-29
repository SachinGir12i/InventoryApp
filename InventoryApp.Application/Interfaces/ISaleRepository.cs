using InventoryApp.Domain.Entities;

namespace InventoryApp.Application.Interfaces
{
    public interface ISaleRepository
    {
        // Create sale + decrease stock + update customer balance
        Task<Sale> CreateAsync(Sale sale);

        // Get single sale with all line items
        Task<Sale?> GetByIdAsync(int id);

        // Get all sales newest first
        Task<List<Sale>> GetAllAsync();

        // Get sales for a specific retailer/customer
        Task<List<Sale>> GetByCustomerIdAsync(int customerId);

        // Generate next sale number e.g. SAL-0001
        Task<string> GenerateSaleNumberAsync();
    }
}