using FluentValidation;
using InventoryApp.Application.Features.Parties.DTOs;

namespace InventoryApp.Application.Features.Parties.Validators
{
    public class CreatePartyValidator : AbstractValidator<CreatePartyDto>
    {
        public CreatePartyValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Party name is required")
                .MaximumLength(100).WithMessage("Name cannot exceed 100 characters");

            RuleFor(x => x.Type)
                .IsInEnum().WithMessage("Invalid party type");

            RuleFor(x => x.Phone)
                .MaximumLength(15).WithMessage("Phone cannot exceed 15 characters")
                .Matches(@"^\d+$").When(x => !string.IsNullOrEmpty(x.Phone))
                .WithMessage("Phone must contain only numbers");

            RuleFor(x => x.Email)
                .EmailAddress().When(x => !string.IsNullOrEmpty(x.Email))
                .WithMessage("Invalid email address");

            RuleFor(x => x.PANNumber)
                .Length(9).When(x => !string.IsNullOrEmpty(x.PANNumber))
                .WithMessage("PAN number must be 9 digits")
                .Matches(@"^\d+$").When(x => !string.IsNullOrEmpty(x.PANNumber))
                .WithMessage("PAN number must contain only numbers");

            RuleFor(x => x.CreditLimit)
                .GreaterThanOrEqualTo(0).WithMessage("Credit limit cannot be negative");

            RuleFor(x => x.ReminderDaysInterval)
                .GreaterThan(0).When(x => x.CreditReminderEnabled)
                .WithMessage("Reminder interval is required when credit reminder is enabled");
        }
    }
}