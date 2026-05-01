using InventoryApp.Application.Features.Invoices.DTOs;

namespace InventoryApp.Application.Interfaces
{
    public interface IInvoiceService
    {
        // Generate PDF bytes from invoice data
        byte[] GenerateInvoicePdf(InvoiceDto invoice);
    }
}