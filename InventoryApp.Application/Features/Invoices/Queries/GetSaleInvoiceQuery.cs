using InventoryApp.Application.Features.Invoices.DTOs;
using InventoryApp.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace InventoryApp.Application.Features.Invoices.Queries
{
    // ── The Query ────────────────────────────────────────────────
    public class GetSaleInvoiceQuery : IRequest<InvoiceDto>
    {
        // Which sale to generate invoice for
        public int SaleId { get; set; }
    }

    // ── The Handler ──────────────────────────────────────────────
    public class GetSaleInvoiceQueryHandler
        : IRequestHandler<GetSaleInvoiceQuery, InvoiceDto>
    {
        private readonly ISaleRepository _saleRepo;
        private readonly IConfiguration _config;

        public GetSaleInvoiceQueryHandler(
            ISaleRepository saleRepo,
            IConfiguration config)
        {
            _saleRepo = saleRepo;
            _config = config;
        }

        public async Task<InvoiceDto> Handle(
            GetSaleInvoiceQuery request,
            CancellationToken cancellationToken)
        {
            // ── Step 1: Get the sale with all details ─────────────
            var sale = await _saleRepo.GetByIdAsync(request.SaleId);

            if (sale == null)
                throw new Exception(
                    $"Sale {request.SaleId} not found");

            // ── Step 2: Build company info from config ────────────
            // These come from appsettings.json
            var company = new CompanyInfoDto
            {
                Name = _config["CompanyInfo:Name"]
                    ?? "My Shoe Factory",
                Address = _config["CompanyInfo:Address"]
                    ?? "Kathmandu, Nepal",
                Phone = _config["CompanyInfo:Phone"],
                Email = _config["CompanyInfo:Email"],
                PANNumber = _config["CompanyInfo:PANNumber"]
            };

            // ── Step 3: Build line items ──────────────────────────
            var lineItems = sale.SaleItems
                .Select((item, index) => new InvoiceLineItemDto
                {
                    SNo = index + 1,
                    ItemName = item.Item?.Name ?? "Unknown",
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    Discount = item.LineDiscount,
                    Amount = item.LineTotal
                }).ToList();

            // ── Step 4: Convert total to words ────────────────────
            string amountInWords =
                ConvertToWords((long)sale.TotalAmount);

            // ── Step 5: Build and return invoice DTO ─────────────
            return new InvoiceDto
            {
                InvoiceNumber = sale.SaleNumber,
                InvoiceDate = sale.SaleDate,
                Company = company,

                CustomerName = sale.Customer?.Name ?? "Unknown",
                CustomerAddress = sale.Customer?.Address,
                CustomerPhone = sale.Customer?.Phone,
                CustomerPAN = sale.Customer?.PANNumber,

                LineItems = lineItems,

                SubTotal = sale.SubTotal,
                DiscountAmount = sale.DiscountAmount,
                VATPercent = sale.VATPercent,
                VATAmount = sale.VATAmount,
                TotalAmount = sale.TotalAmount,
                ReceivedAmount = sale.ReceivedAmount,
                DueAmount = sale.DueAmount,
                AmountInWords = amountInWords,
                PaymentStatus = sale.PaymentStatus.ToString(),
                Remarks = sale.Remarks
            };
        }

        // ── Convert number to words ───────────────────────────────
        // e.g. 22035 → "Twenty Two Thousand Thirty Five Only"
        private string ConvertToWords(long number)
        {
            if (number == 0) return "Zero Only";

            string[] ones = {
                "", "One", "Two", "Three", "Four", "Five",
                "Six", "Seven", "Eight", "Nine", "Ten",
                "Eleven", "Twelve", "Thirteen", "Fourteen",
                "Fifteen", "Sixteen", "Seventeen", "Eighteen",
                "Nineteen"
            };

            string[] tens = {
                "", "", "Twenty", "Thirty", "Forty", "Fifty",
                "Sixty", "Seventy", "Eighty", "Ninety"
            };

            if (number < 0)
                return "Minus " + ConvertToWords(-number);

            if (number < 20)
                return ones[number];

            if (number < 100)
                return tens[number / 10] +
                    (number % 10 > 0
                        ? " " + ones[number % 10]
                        : "");

            if (number < 1000)
                return ones[number / 100] + " Hundred" +
                    (number % 100 > 0
                        ? " " + ConvertToWords(number % 100)
                        : "");

            if (number < 100000)
                return ConvertToWords(number / 1000) +
                    " Thousand" +
                    (number % 1000 > 0
                        ? " " + ConvertToWords(number % 1000)
                        : "");

            if (number < 10000000)
                return ConvertToWords(number / 100000) +
                    " Lakh" +
                    (number % 100000 > 0
                        ? " " + ConvertToWords(number % 100000)
                        : "");

            return ConvertToWords(number / 10000000) +
                " Crore" +
                (number % 10000000 > 0
                    ? " " + ConvertToWords(number % 10000000)
                    : "");
        }
    }
}