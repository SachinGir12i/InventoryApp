using InventoryApp.Application.Features.Purchases.Commands;
using InventoryApp.Application.Features.Purchases.DTOs;
using InventoryApp.Application.Features.Purchases.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace InventoryApp.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PurchasesController : ControllerBase
    {
        private readonly IMediator _mediator;
        public PurchasesController(IMediator mediator) => _mediator = mediator;

        // POST api/purchases
        // Record buying raw materials from a supplier
        [HttpPost]
        public async Task<IActionResult> CreatePurchase(
            [FromBody] CreatePurchaseDto dto)
        {
            var id = await _mediator.Send(
                new CreatePurchaseCommand { Purchase = dto });

            return Ok(new { Id = id, Message = "Purchase recorded successfully" });
        }

        // GET api/purchases
        // Get all purchases, newest first
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllPurchasesQuery());
            return Ok(result);
        }

        // GET api/purchases/1
        // Get a single purchase with all its line items
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(
                new GetPurchaseByIdQuery { Id = id });

            if (result == null)
                return NotFound(new { Message = $"Purchase {id} not found" });

            return Ok(result);
        }

        // GET api/purchases/supplier/1
        // Get all purchases from a specific supplier
        [HttpGet("supplier/{supplierId}")]
        public async Task<IActionResult> GetBySupplier(int supplierId)
        {
            var result = await _mediator.Send(
                new GetPurchasesBySupplierQuery { SupplierId = supplierId });

            return Ok(result);
        }
    }
}