using InventoryApp.Application.Features.ItemCategories.DTOs;
using InventoryApp.Application.Interfaces;
using InventoryApp.Domain.Entities;
using MediatR;

namespace InventoryApp.Application.Features.ItemCategories.Commands
{
    public class CreateItemCategoryCommand : IRequest<int>
    {
        public CreateItemCategoryDto Category { get; set; } = null!;
    }

    public class CreateItemCategoryCommandHandler : IRequestHandler<CreateItemCategoryCommand, int>
    {
        private readonly IItemCategoryRepository _repo;
        public CreateItemCategoryCommandHandler(IItemCategoryRepository repo) => _repo = repo;

        public async Task<int> Handle(CreateItemCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = new ItemCategory
            {
                Name = request.Category.Name,
                Description = request.Category.Description
            };

            var created = await _repo.CreateAsync(category);
            return created.Id;
        }
    }
}