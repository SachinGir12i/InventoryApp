using InventoryApp.Application.Features.Sales.Commands;
using InventoryApp.Application.Features.Sales.DTOs;
using InventoryApp.Application.Features.Sales.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace InventoryApp.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SalesController : ControllerBase
    {
        private readonly IMediator _mediator;
        public SalesController(IMediator mediator) => _mediator = mediator;

        // POST api/sales
        // Record selling finished shoes to a retailer
        [HttpPost]
        public async Task<IActionResult> CreateSale(
            [FromBody] CreateSaleDto dto)
        {
            var id = await _mediator.Send(
                new CreateSaleCommand { Sale = dto });

            return Ok(new { Id = id, Message = "Sale recorded successfully" });
        }

        // GET api/sales
        // Get all sales newest first
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllSalesQuery());
            return Ok(result);
        }

        // GET api/sales/1
        // Get a single sale with all its line items
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(
                new GetSaleByIdQuery { Id = id });

            if (result == null)
                return NotFound(new { Message = $"Sale {id} not found" });

            return Ok(result);
        }

        // GET api/sales/customer/1
        // Get all sales to a specific retailer
        [HttpGet("customer/{customerId}")]
        public async Task<IActionResult> GetByCustomer(int customerId)
        {
            var result = await _mediator.Send(
                new GetSalesByCustomerQuery { CustomerId = customerId });

            return Ok(result);
        }
    }
}