using FluentValidation;
using InventoryApp.Application.Features.ItemCategories.DTOs;

namespace InventoryApp.Application.Features.ItemCategories.Validators
{
    public class CreateItemCategoryValidator : AbstractValidator<CreateItemCategoryDto>
    {
        public CreateItemCategoryValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Category name is required")
                .MaximumLength(50).WithMessage("Category name cannot exceed 50 characters");
        }
    }
}