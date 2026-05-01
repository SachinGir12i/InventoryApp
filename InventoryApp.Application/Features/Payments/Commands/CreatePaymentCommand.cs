using InventoryApp.Application.Features.Payments.DTOs;
using InventoryApp.Application.Interfaces;
using InventoryApp.Domain.Entities;
using InventoryApp.Domain.Enums;
using MediatR;

namespace InventoryApp.Application.Features.Payments.Commands
{
    // ── The Command ──────────────────────────────────────────────
    public class CreatePaymentCommand : IRequest<int>
    {
        public CreatePaymentDto Payment { get; set; } = null!;
    }

    // ── The Handler ──────────────────────────────────────────────
    public class CreatePaymentCommandHandler
        : IRequestHandler<CreatePaymentCommand, int>
    {
        private readonly IPaymentRepository _paymentRepo;

        public CreatePaymentCommandHandler(IPaymentRepository paymentRepo)
        {
            _paymentRepo = paymentRepo;
        }

        public async Task<int> Handle(
            CreatePaymentCommand request,
            CancellationToken cancellationToken)
        {
            var dto = request.Payment;

            // ── Generate payment number ───────────────────────────
            var paymentNumber = await _paymentRepo
                .GeneratePaymentNumberAsync();

            // ── Build the Payment entity ─────────────────────────
            var payment = new Payment
            {
                PaymentNumber = paymentNumber,
                PaymentDate = dto.PaymentDate,
                PartyId = dto.PartyId,
                PaymentType = dto.PaymentType,
                Amount = dto.Amount,
                PaymentMethod = dto.PaymentMethod,
                SaleId = dto.SaleId,
                PurchaseId = dto.PurchaseId,
                TransactionReference = dto.TransactionReference,
                Remarks = dto.Remarks
            };

            // ── Save payment + update party balance ──────────────
            // The repository handles the balance update
            var created = await _paymentRepo.CreateAsync(payment);
            return created.Id;
        }
    }
}