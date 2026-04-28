using InventoryApp.Application.Features.Items.Commands;
using InventoryApp.Application.Features.Items.DTOs;
using InventoryApp.Application.Features.Items.Queries;
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

        // POST api/items
        [HttpPost]
        public async Task<IActionResult> CreateItem([FromBody] CreateItemDto dto)
        {
            var command = new CreateItemCommand { Item = dto };
            var itemId = await _mediator.Send(command);
            return Ok(new { Id = itemId, Message = "Item created successfully" });
        }

        // GET api/items
        [HttpGet]
        public async Task<IActionResult> GetAllItems()
        {
            var query = new GetAllItemsQuery();
            var items = await _mediator.Send(query);
            return Ok(items);
        }

        // GET api/items/1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetItemById(int id)
        {
            var query = new GetItemByIdQuery { Id = id };
            var item = await _mediator.Send(query);

            if (item == null)
                return NotFound(new { Message = $"Item with Id {id} was not found" });

            return Ok(item);
        }
        // POST api/items/prices
        [HttpPost("prices")]
        public async Task<IActionResult> AddPrice([FromBody] AddItemPriceDto dto)
        {
            var id = await _mediator.Send(new AddItemPriceCommand { ItemPrice = dto });
            return Ok(new { Id = id, Message = "Price added successfully" });
        }

        // GET api/items/1/prices
        [HttpGet("{itemId}/prices")]
        public async Task<IActionResult> GetPrices(int itemId)
        {
            var result = await _mediator.Send(new GetItemPricesQuery { ItemId = itemId });
            return Ok(result);
        }
    }
}