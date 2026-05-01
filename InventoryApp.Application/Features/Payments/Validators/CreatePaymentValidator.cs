using FluentValidation;
using InventoryApp.Application.Features.Payments.DTOs;

namespace InventoryApp.Application.Features.Payments.Validators
{
    public class CreatePaymentValidator : AbstractValidator<CreatePaymentDto>
    {
        public CreatePaymentValidator()
        {
            // Must specify which party this payment is with
            RuleFor(x => x.PartyId)
                .GreaterThan(0).WithMessage("Party is required");

            // Amount must be positive
            RuleFor(x => x.Amount)
                .GreaterThan(0).WithMessage("Payment amount must be greater than 0");

            // Payment type must be valid
            RuleFor(x => x.PaymentType)
                .IsInEnum().WithMessage("Invalid payment type");

            // Payment method must be valid
            RuleFor(x => x.PaymentMethod)
                .IsInEnum().WithMessage("Invalid payment method");

            // Transaction reference required for bank transfers
            RuleFor(x => x.TransactionReference)
                .NotEmpty()
                .When(x => x.PaymentMethod ==
                    Domain.Enums.PaymentMethod.BankTransfer)
                .WithMessage(
                    "Transaction reference is required for bank transfers");
        }
    }
}