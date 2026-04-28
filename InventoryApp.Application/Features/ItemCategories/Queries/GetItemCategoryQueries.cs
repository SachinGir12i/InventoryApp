using InventoryApp.Application.Features.ItemCategories.DTOs;
using InventoryApp.Application.Interfaces;
using MediatR;

namespace InventoryApp.Application.Features.ItemCategories.Queries
{
    // ─── Get All ─────────────────────────────────────────────

    public class GetAllItemCategoriesQuery : IRequest<List<ItemCategoryResponseDto>> { }

    public class GetAllItemCategoriesQueryHandler
        : IRequestHandler<GetAllItemCategoriesQuery, List<ItemCategoryResponseDto>>
    {
        private readonly IItemCategoryRepository _repo;
        public GetAllItemCategoriesQueryHandler(IItemCategoryRepository repo) => _repo = repo;

        public async Task<List<ItemCategoryResponseDto>> Handle(
            GetAllItemCategoriesQuery request, CancellationToken cancellationToken)
        {
            var categories = await _repo.GetAllAsync();
            return categories.Select(c => new ItemCategoryResponseDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                ItemCount = c.Items.Count(i => i.IsActive),
                IsActive = c.IsActive,
                CreatedAt = c.CreatedAt
            }).ToList();
        }
    }

    // ─── Get By Id ───────────────────────────────────────────

    public class GetItemCategoryByIdQuery : IRequest<ItemCategoryResponseDto?>
    {
        public int Id { get; set; }
    }

    public class GetItemCategoryByIdQueryHandler
        : IRequestHandler<GetItemCategoryByIdQuery, ItemCategoryResponseDto?>
    {
        private readonly IItemCategoryRepository _repo;
        public GetItemCategoryByIdQueryHandler(IItemCategoryRepository repo) => _repo = repo;

        public async Task<ItemCategoryResponseDto?> Handle(
            GetItemCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            var c = await _repo.GetByIdAsync(request.Id);
            if (c == null) return null;

            return new ItemCategoryResponseDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                ItemCount = c.Items.Count(i => i.IsActive),
                IsActive = c.IsActive,
                CreatedAt = c.CreatedAt
            };
        }
    }
}