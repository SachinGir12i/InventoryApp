using InventoryApp.Application.Features.Payments.DTOs;
using InventoryApp.Application.Interfaces;
using InventoryApp.Domain.Enums;
using MediatR;

namespace InventoryApp.Application.Features.Payments.Queries
{
    // ── Helper mapper ────────────────────────────────────────────
    internal static class PaymentMapper
    {
        public static PaymentResponseDto ToDto(Domain.Entities.Payment p) => new()
        {
            Id = p.Id,
            PaymentNumber = p.PaymentNumber,
            PaymentDate = p.PaymentDate,
            PartyId = p.PartyId,
            PartyName = p.Party?.Name ?? string.Empty,
            PaymentType = p.PaymentType.ToString(),
            Amount = p.Amount,
            PaymentMethod = p.PaymentMethod.ToString(),
            SaleId = p.SaleId,

            // Show sale number if linked to a sale
            SaleNumber = p.Sale?.SaleNumber,
            PurchaseId = p.PurchaseId,

            // Show purchase number if linked to a purchase
            PurchaseNumber = p.Purchase?.PurchaseNumber,
            TransactionReference = p.TransactionReference,
            Remarks = p.Remarks,
            CreatedAt = p.CreatedAt
        };
    }

    // ── Get All Payments ─────────────────────────────────────────
    public class GetAllPaymentsQuery : IRequest<List<PaymentResponseDto>> { }

    public class GetAllPaymentsQueryHandler
        : IRequestHandler<GetAllPaymentsQuery, List<PaymentResponseDto>>
    {
        private readonly IPaymentRepository _repo;
        public GetAllPaymentsQueryHandler(IPaymentRepository repo) => _repo = repo;

        public async Task<List<PaymentResponseDto>> Handle(
            GetAllPaymentsQuery request, CancellationToken cancellationToken)
        {
            var payments = await _repo.GetAllAsync();
            return payments.Select(PaymentMapper.ToDto).ToList();
        }
    }

    // ── Get Payment By Id ────────────────────────────────────────
    public class GetPaymentByIdQuery : IRequest<PaymentResponseDto?>
    {
        public int Id { get; set; }
    }

    public class GetPaymentByIdQueryHandler
        : IRequestHandler<GetPaymentByIdQuery, PaymentResponseDto?>
    {
        private readonly IPaymentRepository _repo;
        public GetPaymentByIdQueryHandler(IPaymentRepository repo) => _repo = repo;

        public async Task<PaymentResponseDto?> Handle(
            GetPaymentByIdQuery request, CancellationToken cancellationToken)
        {
            var payment = await _repo.GetByIdAsync(request.Id);
            return payment == null ? null : PaymentMapper.ToDto(payment);
        }
    }

    // ── Get Payments By Party ────────────────────────────────────
    // Useful for seeing full payment history of a retailer
    public class GetPaymentsByPartyQuery : IRequest<List<PaymentResponseDto>>
    {
        public int PartyId { get; set; }
    }

    public class GetPaymentsByPartyQueryHandler
        : IRequestHandler<GetPaymentsByPartyQuery, List<PaymentResponseDto>>
    {
        private readonly IPaymentRepository _repo;
        public GetPaymentsByPartyQueryHandler(IPaymentRepository repo) => _repo = repo;

        public async Task<List<PaymentResponseDto>> Handle(
            GetPaymentsByPartyQuery request, CancellationToken cancellationToken)
        {
            var payments = await _repo.GetByPartyIdAsync(request.PartyId);
            return payments.Select(PaymentMapper.ToDto).ToList();
        }
    }

    // ── Get Payments By Type ─────────────────────────────────────
    // e.g. get only money received from retailers
    public class GetPaymentsByTypeQuery : IRequest<List<PaymentResponseDto>>
    {
        public PaymentType PaymentType { get; set; }
    }

    public class GetPaymentsByTypeQueryHandler
        : IRequestHandler<GetPaymentsByTypeQuery, List<PaymentResponseDto>>
    {
        private readonly IPaymentRepository _repo;
        public GetPaymentsByTypeQueryHandler(IPaymentRepository repo) => _repo = repo;

        public async Task<List<PaymentResponseDto>> Handle(
            GetPaymentsByTypeQuery request, CancellationToken cancellationToken)
        {
            var payments = await _repo.GetByTypeAsync(request.PaymentType);
            return payments.Select(PaymentMapper.ToDto).ToList();
        }
    }
}