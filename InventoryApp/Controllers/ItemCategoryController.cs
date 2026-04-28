using InventoryApp.Application.Features.ItemCategories.Commands;
using InventoryApp.Application.Features.ItemCategories.DTOs;
using InventoryApp.Application.Features.ItemCategories.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace InventoryApp.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemCategoriesController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ItemCategoriesController(IMediator mediator) => _mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateItemCategoryDto dto)
        {
            var id = await _mediator.Send(new CreateItemCategoryCommand { Category = dto });
            return Ok(new { Id = id, Message = "Category created successfully" });
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllItemCategoriesQuery());
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetItemCategoryByIdQuery { Id = id });
            if (result == null)
                return NotFound(new { Message = $"Category with Id {id} not found" });
            return Ok(result);
        }
    }
}