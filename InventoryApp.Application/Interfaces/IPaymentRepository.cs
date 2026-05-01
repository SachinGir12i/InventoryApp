using InventoryApp.Domain.Entities;
using InventoryApp.Domain.Enums;

namespace InventoryApp.Application.Interfaces
{
    public interface IPaymentRepository
    {
        // Record payment + update party balance
        Task<Payment> CreateAsync(Payment payment);

        // Get single payment
        Task<Payment?> GetByIdAsync(int id);

        // Get all payments newest first
        Task<List<Payment>> GetAllAsync();

        // Get all payments for a specific party (retailer or supplier)
        Task<List<Payment>> GetByPartyIdAsync(int partyId);

        // Get payments by type (received or paid)
        Task<List<Payment>> GetByTypeAsync(PaymentType type);

        // Generate next payment number e.g. PAY-0001
        Task<string> GeneratePaymentNumberAsync();
    }
}