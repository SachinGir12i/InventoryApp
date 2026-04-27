using InventoryApp.Application.Features.Parties.Commands;
using InventoryApp.Application.Features.Parties.DTOs;
using InventoryApp.Application.Features.Parties.Queries;
using InventoryApp.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace InventoryApp.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PartiesController : ControllerBase
    {
        private readonly IMediator _mediator;
        public PartiesController(IMediator mediator) => _mediator = mediator;

        // POST api/parties
        [HttpPost]
        public async Task<IActionResult> CreateParty([FromBody] CreatePartyDto dto)
        {
            var id = await _mediator.Send(new CreatePartyCommand { Party = dto });
            return Ok(new { Id = id, Message = "Party created successfully" });
        }

        // GET api/parties
        [HttpGet]
        public async Task<IActionResult> GetAllParties()
        {
            var result = await _mediator.Send(new GetAllPartiesQuery());
            return Ok(result);
        }

        // GET api/parties/1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPartyById(int id)
        {
            var result = await _mediator.Send(new GetPartyByIdQuery { Id = id });
            if (result == null)
                return NotFound(new { Message = $"Party with Id {id} not found" });
            return Ok(result);
        }

        // GET api/parties/type/Customer
        [HttpGet("type/{type}")]
        public async Task<IActionResult> GetByType(PartyType type)
        {
            var result = await _mediator.Send(new GetPartiesByTypeQuery { Type = type });
            return Ok(result);
        }

        // GET api/parties/search?term=ram
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string term)
        {
            var result = await _mediator.Send(new SearchPartiesQuery { SearchTerm = term });
            return Ok(result);
        }

        // DELETE api/parties/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteParty(int id)
        {
            // we'll add DeletePartyCommand in the next step
            return Ok();
        }
    }
}