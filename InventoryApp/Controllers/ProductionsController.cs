using InventoryApp.Application.Features.Productions.Commands;
using InventoryApp.Application.Features.Productions.DTOs;
using InventoryApp.Application.Features.Productions.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace InventoryApp.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductionsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ProductionsController(IMediator mediator) => _mediator = mediator;

        // POST api/productions
        // Record a production batch
        // Raw materials go DOWN, finished shoes go UP
        [HttpPost]
        public async Task<IActionResult> CreateProduction(
            [FromBody] CreateProductionDto dto)
        {
            var id = await _mediator.Send(
                new CreateProductionCommand { Production = dto });

            return Ok(new { Id = id, Message = "Production recorded successfully" });
        }

        // GET api/productions
        // Get all production batches newest first
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllProductionsQuery());
            return Ok(result);
        }

        // GET api/productions/1
        // Get single production with full details
        // including all materials used and shoes produced
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(
                new GetProductionByIdQuery { Id = id });

            if (result == null)
                return NotFound(new { Message = $"Production {id} not found" });

            return Ok(result);
        }
    }
}