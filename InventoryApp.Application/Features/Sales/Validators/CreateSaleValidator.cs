using FluentValidation;
using InventoryApp.Application.Features.Sales.DTOs;

namespace InventoryApp.Application.Features.Sales.Validators
{
    public class CreateSaleValidator : AbstractValidator<CreateSaleDto>
    {
        public CreateSaleValidator()
        {
            // Must have a customer
            RuleFor(x => x.CustomerId)
                .GreaterThan(0).WithMessage("Customer is required");

            // Must have at least one item to sell
            RuleFor(x => x.Items)
                .NotEmpty().WithMessage("Sale must have at least one item");

            // Discount cannot be negative
            RuleFor(x => x.DiscountAmount)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Discount cannot be negative");

            // Received amount cannot be negative
            RuleFor(x => x.ReceivedAmount)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Received amount cannot be negative");

            // Validate each line item
            RuleForEach(x => x.Items).ChildRules(item =>
            {
                item.RuleFor(x => x.ItemId)
                    .GreaterThan(0).WithMessage("Item is required");

                item.RuleFor(x => x.Quantity)
                    .GreaterThan(0).WithMessage("Quantity must be greater than 0");

                item.RuleFor(x => x.UnitPrice)
                    .GreaterThan(0).WithMessage("Unit price must be greater than 0");

                item.RuleFor(x => x.LineDiscount)
                    .GreaterThanOrEqualTo(0)
                    .WithMessage("Line discount cannot be negative");
            });
        }
    }
}