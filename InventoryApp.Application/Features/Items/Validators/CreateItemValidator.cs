using FluentValidation;
using InventoryApp.Application.Features.Items.DTOs;

namespace InventoryApp.Application.Features.Items.Validators
{
    public class CreateItemValidator : AbstractValidator<CreateItemDto>
    {
        public CreateItemValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Item name is required")
                .MaximumLength(100).WithMessage("Name cannot exceed 100 characters");

            RuleFor(x => x.CostPrice)
                .GreaterThan(0).WithMessage("Cost price must be greater than 0");

            RuleFor(x => x.SellingPrice)
                .GreaterThan(0).WithMessage("Selling price must be greater than 0");

            RuleFor(x => x.SKU)
                .MaximumLength(50).WithMessage("SKU cannot exceed 50 characters");
        }
    }
}