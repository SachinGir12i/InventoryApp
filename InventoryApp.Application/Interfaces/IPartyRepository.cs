using InventoryApp.Domain.Entities;
using InventoryApp.Domain.Enums;

namespace InventoryApp.Application.Interfaces
{
    public interface IPartyRepository
    {
        Task<Party> CreateAsync(Party party);
        Task<Party?> GetByIdAsync(int id);
        Task<List<Party>> GetAllAsync();
        Task<List<Party>> GetByTypeAsync(PartyType type);  // get only customers or suppliers
        Task<List<Party>> SearchByNameOrAddressAsync(string searchTerm);
        Task<Party> UpdateAsync(Party party);
        Task<bool> DeleteAsync(int id);
    }
}