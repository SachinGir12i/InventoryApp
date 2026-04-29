using FluentValidation;
using InventoryApp.Application.Features.Purchases.DTOs;

namespace InventoryApp.Application.Features.Purchases.Validators
{
    public class CreatePurchaseValidator : AbstractValidator<CreatePurchaseDto>
    {
        public CreatePurchaseValidator()
        {
            // Must have a supplier
            RuleFor(x => x.SupplierId)
                .GreaterThan(0).WithMessage("Supplier is required");

            // Must have at least one item
            RuleFor(x => x.Items)
                .NotEmpty().WithMessage("Purchase must have at least one item");

            // VAT must be 0 or 13 for Nepal
            RuleFor(x => x.VATPercent)
                .InclusiveBetween(0, 100)
                .WithMessage("VAT percent must be between 0 and 100");

            // Paid amount cannot be negative
            RuleFor(x => x.PaidAmount)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Paid amount cannot be negative");

            // Validate each line item
            RuleForEach(x => x.Items).ChildRules(item =>
            {
                item.RuleFor(x => x.ItemId)
                    .GreaterThan(0).WithMessage("Item is required");

                item.RuleFor(x => x.Quantity)
                    .GreaterThan(0).WithMessage("Quantity must be greater than 0");

                item.RuleFor(x => x.UnitPrice)
                    .GreaterThan(0).WithMessage("Unit price must be greater than 0");
            });
        }
    }
}