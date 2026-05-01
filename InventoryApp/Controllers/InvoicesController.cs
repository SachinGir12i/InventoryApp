using InventoryApp.Application.Features.Invoices.Queries;
using InventoryApp.Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryApp.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class InvoicesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IInvoiceService _invoiceService;

        public InvoicesController(
            IMediator mediator,
            IInvoiceService invoiceService)
        {
            _mediator = mediator;
            _invoiceService = invoiceService;
        }

        // GET api/invoices/sale/1
        // Returns PDF file for a sale
        // React can open this in a new tab or download it
        [HttpGet("sale/{saleId}")]
        public async Task<IActionResult> GetSaleInvoice(int saleId)
        {
            try
            {
                // Step 1: Get invoice data from the sale
                var invoiceDto = await _mediator.Send(
                    new GetSaleInvoiceQuery { SaleId = saleId });

                // Step 2: Generate PDF bytes
                var pdfBytes = _invoiceService
                    .GenerateInvoicePdf(invoiceDto);

                // Step 3: Return as downloadable PDF file
                // The browser will open or download this
                return File(
                    pdfBytes,
                    "application/pdf",
                    $"Invoice-{invoiceDto.InvoiceNumber}.pdf");
            }
            catch (Exception ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }

        // GET api/invoices/sale/1/preview
        // Returns invoice data as JSON (for React to preview)
        // before generating the actual PDF
        [HttpGet("sale/{saleId}/preview")]
        public async Task<IActionResult> PreviewSaleInvoice(
            int saleId)
        {
            try
            {
                var invoiceDto = await _mediator.Send(
                    new GetSaleInvoiceQuery { SaleId = saleId });

                return Ok(invoiceDto);
            }
            catch (Exception ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }
    }
}