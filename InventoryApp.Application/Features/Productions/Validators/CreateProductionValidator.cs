using FluentValidation;
using InventoryApp.Application.Features.Productions.DTOs;

namespace InventoryApp.Application.Features.Productions.Validators
{
    public class CreateProductionValidator
        : AbstractValidator<CreateProductionDto>
    {
        public CreateProductionValidator()
        {
            // Must have at least one raw material being consumed
            RuleFor(x => x.MaterialsUsed)
                .NotEmpty()
                .WithMessage("At least one raw material is required");

            // Must produce at least one finished product
            RuleFor(x => x.OutputItems)
                .NotEmpty()
                .WithMessage("At least one output item is required");

            // Labor cost cannot be negative
            RuleFor(x => x.LaborCost)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Labor cost cannot be negative");

            // Other cost cannot be negative
            RuleFor(x => x.OtherCost)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Other cost cannot be negative");

            // Validate each material
            RuleForEach(x => x.MaterialsUsed).ChildRules(m =>
            {
                m.RuleFor(x => x.ItemId)
                    .GreaterThan(0).WithMessage("Raw material is required");

                m.RuleFor(x => x.QuantityUsed)
                    .GreaterThan(0)
                    .WithMessage("Quantity used must be greater than 0");

                m.RuleFor(x => x.UnitCost)
                    .GreaterThanOrEqualTo(0)
                    .WithMessage("Unit cost cannot be negative");
            });

            // Validate each output
            RuleForEach(x => x.OutputItems).ChildRules(o =>
            {
                o.RuleFor(x => x.ItemId)
                    .GreaterThan(0)
                    .WithMessage("Output item is required");

                o.RuleFor(x => x.QuantityProduced)
                    .GreaterThan(0)
                    .WithMessage("Quantity produced must be greater than 0");
            });
        }
    }
}