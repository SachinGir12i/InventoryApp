using InventoryApp.Application.Interfaces;
using InventoryApp.Domain.Entities;
using InventoryApp.Domain.Enums;
using InventoryApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace InventoryApp.Infrastructure.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly AppDbContext _context;
        public PaymentRepository(AppDbContext context) => _context = context;

        public async Task<Payment> CreateAsync(Payment payment)
        {
            // ── Transaction: save payment + update balance together ─
            using var transaction = await _context.Database
                .BeginTransactionAsync();

            try
            {
                // Step 1: Save the payment record
                _context.Payments.Add(payment);
                await _context.SaveChangesAsync();

                // Step 2: Update the party balance
                var party = await _context.Parties
                    .FindAsync(payment.PartyId);

                if (party != null)
                {
                    if (payment.PaymentType == PaymentType.Received)
                    {
                        // Retailer paid us → reduce what they owe us
                        // Their balance was positive (they owed us)
                        // Payment reduces that balance
                        party.CurrentBalance -= payment.Amount;
                    }
                    else if (payment.PaymentType == PaymentType.Paid)
                    {
                        // We paid supplier → reduce what we owe them
                        // Their balance was negative (we owed them)
                        // Payment brings balance back toward zero
                        party.CurrentBalance += payment.Amount;
                    }

                    party.UpdatedAt = DateTime.UtcNow;
                }

                // Step 3: Save balance update
                await _context.SaveChangesAsync();

                // Step 4: Commit everything
                await transaction.CommitAsync();

                return payment;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<Payment?> GetByIdAsync(int id)
        {
            return await _context.Payments
                // Load party name
                .Include(p => p.Party)
                // Load sale number if linked
                .Include(p => p.Sale)
                // Load purchase number if linked
                .Include(p => p.Purchase)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<Payment>> GetAllAsync()
        {
            return await _context.Payments
                .Include(p => p.Party)
                .Include(p => p.Sale)
                .Include(p => p.Purchase)
                // Newest payments first
                .OrderByDescending(p => p.PaymentDate)
                .ToListAsync();
        }

        public async Task<List<Payment>> GetByPartyIdAsync(int partyId)
        {
            return await _context.Payments
                .Include(p => p.Party)
                .Include(p => p.Sale)
                .Include(p => p.Purchase)
                .Where(p => p.PartyId == partyId)
                .OrderByDescending(p => p.PaymentDate)
                .ToListAsync();
        }

        public async Task<List<Payment>> GetByTypeAsync(PaymentType type)
        {
            return await _context.Payments
                .Include(p => p.Party)
                .Include(p => p.Sale)
                .Include(p => p.Purchase)
                .Where(p => p.PaymentType == type)
                .OrderByDescending(p => p.PaymentDate)
                .ToListAsync();
        }

        public async Task<string> GeneratePaymentNumberAsync()
        {
            // e.g. PAY-0001, PAY-0002...
            var count = await _context.Payments.CountAsync();
            return $"PAY-{(count + 1):D4}";
        }
    }
}