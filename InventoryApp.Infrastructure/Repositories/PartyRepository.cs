using InventoryApp.Application.Interfaces;
using InventoryApp.Domain.Entities;
using InventoryApp.Domain.Enums;
using InventoryApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace InventoryApp.Infrastructure.Repositories
{
    public class PartyRepository : IPartyRepository
    {
        private readonly AppDbContext _context;
        public PartyRepository(AppDbContext context) => _context = context;

        public async Task<Party> CreateAsync(Party party)
        {
            _context.Parties.Add(party);
            await _context.SaveChangesAsync();
            return party;
        }

        public async Task<Party?> GetByIdAsync(int id)
        {
            return await _context.Parties
                .Include(p => p.Category)   // load category name too
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<Party>> GetAllAsync()
        {
            return await _context.Parties
                .Include(p => p.Category)
                .Where(p => p.IsActive)
                .OrderBy(p => p.Name)
                .ToListAsync();
        }

        public async Task<List<Party>> GetByTypeAsync(PartyType type)
        {
            return await _context.Parties
                .Include(p => p.Category)
                .Where(p => p.Type == type || p.Type == PartyType.Both)
                .Where(p => p.IsActive)
                .OrderBy(p => p.Name)
                .ToListAsync();
        }

        public async Task<List<Party>> SearchByNameOrAddressAsync(string searchTerm)
        {
            return await _context.Parties
                .Include(p => p.Category)
                .Where(p => p.Name.Contains(searchTerm) ||
                            (p.Address != null && p.Address.Contains(searchTerm)) ||
                            (p.Phone != null && p.Phone.Contains(searchTerm)))
                .Where(p => p.IsActive)
                .ToListAsync();
        }

        public async Task<Party> UpdateAsync(Party party)
        {
            party.UpdatedAt = DateTime.UtcNow;
            _context.Parties.Update(party);
            await _context.SaveChangesAsync();
            return party;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var party = await _context.Parties.FindAsync(id);
            if (party == null) return false;

            party.IsActive = false;         // soft delete - don't actually remove from DB
            party.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}