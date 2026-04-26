using InventoryApp.Application.Features.Items.Commands;
using InventoryApp.Application.Features.Items.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace InventoryApp.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ItemsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateItem([FromBody] CreateItemDto dto)
        {
            var command = new CreateItemCommand { Item = dto };
            var itemId = await _mediator.Send(command);
            return Ok(new { Id = itemId, Message = "Item created successfully" });
        }
    }
}