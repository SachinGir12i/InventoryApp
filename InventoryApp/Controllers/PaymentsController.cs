using InventoryApp.Application.Features.Payments.Commands;
using InventoryApp.Application.Features.Payments.DTOs;
using InventoryApp.Application.Features.Payments.Queries;
using InventoryApp.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace InventoryApp.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public PaymentsController(IMediator mediator) => _mediator = mediator;

        // POST api/payments
        // Record a payment received from retailer or paid to supplier
        [HttpPost]
        public async Task<IActionResult> CreatePayment(
            [FromBody] CreatePaymentDto dto)
        {
            var id = await _mediator.Send(
                new CreatePaymentCommand { Payment = dto });

            return Ok(new { Id = id, Message = "Payment recorded successfully" });
        }

        // GET api/payments
        // Get all payments newest first
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllPaymentsQuery());
            return Ok(result);
        }

        // GET api/payments/1
        // Get single payment with full details
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(
                new GetPaymentByIdQuery { Id = id });

            if (result == null)
                return NotFound(new { Message = $"Payment {id} not found" });

            return Ok(result);
        }

        // GET api/payments/party/1
        // Get all payment history for a specific retailer or supplier
        // Very useful for checking who still owes you money
        [HttpGet("party/{partyId}")]
        public async Task<IActionResult> GetByParty(int partyId)
        {
            var result = await _mediator.Send(
                new GetPaymentsByPartyQuery { PartyId = partyId });

            return Ok(result);
        }

        // GET api/payments/type/1
        // 1 = Received (from retailers), 2 = Paid (to suppliers)
        [HttpGet("type/{type}")]
        public async Task<IActionResult> GetByType(PaymentType type)
        {
            var result = await _mediator.Send(
                new GetPaymentsByTypeQuery { PaymentType = type });

            return Ok(result);
        }
    }
}