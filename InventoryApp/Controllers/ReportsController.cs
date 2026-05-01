using InventoryApp.Application.Features.Reports.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryApp.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ReportsController(IMediator mediator) => _mediator = mediator;

        // GET api/reports/stock
        // Full stock report with summary, raw materials,
        // finished goods and alerts all in one response
        // GET api/reports/stock?categoryId=1
        [HttpGet("stock")]
        public async Task<IActionResult> GetStockReport([FromQuery] int? categoryId = null)
        {
            var result = await _mediator.Send(
                new GetStockReportQuery { CategoryId = categoryId });

            return Ok(result);
        }

        // GET api/reports/stock/low
        // Just the low stock alerts
        // Use this for dashboard notification badge
        [HttpGet("stock/low")]
        public async Task<IActionResult> GetLowStockAlerts()
        {
            var result = await _mediator.Send(
                new GetLowStockAlertsQuery());

            // Return count in header so React can show badge number
            Response.Headers.Append(
                "X-Low-Stock-Count",
                result.Count.ToString());

            return Ok(result);
        }
        // GET api/reports/profit-loss
        // GET api/reports/profit-loss?fromDate=2026-01-01&toDate=2026-04-30
        // Full P&L report for the given period
        [HttpGet("profit-loss")]
        [Authorize(Roles = "Owner,Admin,Accountant")]
        public async Task<IActionResult> GetProfitLossReport(
            [FromQuery] DateTime? fromDate = null,
            [FromQuery] DateTime? toDate = null)
        {
            var query = new GetProfitLossReportQuery
            {
                FromDate = fromDate ?? new DateTime(
                    DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1),
                ToDate = toDate ?? DateTime.UtcNow
            };

            var result = await _mediator.Send(query);
            return Ok(result);
        }
        // GET api/reports/vat
        // GET api/reports/vat?fromDate=2026-04-01&toDate=2026-04-30
        // Monthly VAT report for IRD submission
        // Only Owner, Admin and Accountant can view this
        [HttpGet("vat")]
        [Authorize(Roles = "Owner,Admin,Accountant")]
        public async Task<IActionResult> GetVATReport(
            [FromQuery] DateTime? fromDate = null,
            [FromQuery] DateTime? toDate = null)
        {
            var query = new GetVATReportQuery
            {
                FromDate = fromDate ?? new DateTime(
                    DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1),
                ToDate = toDate ?? DateTime.UtcNow
            };

            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}