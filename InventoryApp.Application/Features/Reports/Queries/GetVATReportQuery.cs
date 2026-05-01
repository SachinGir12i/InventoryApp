using InventoryApp.Application.Features.Reports.DTOs;
using InventoryApp.Application.Interfaces;
using MediatR;

namespace InventoryApp.Application.Features.Reports.Queries
{
    // ── The Query ────────────────────────────────────────────────
    public class GetVATReportQuery : IRequest<VATReportDto>
    {
        // Default to current month
        public DateTime FromDate { get; set; }
            = new DateTime(DateTime.UtcNow.Year,
                DateTime.UtcNow.Month, 1);

        public DateTime ToDate { get; set; }
            = DateTime.UtcNow;
    }

    // ── The Handler ──────────────────────────────────────────────
    public class GetVATReportQueryHandler
        : IRequestHandler<GetVATReportQuery, VATReportDto>
    {
        private readonly ISaleRepository _saleRepo;
        private readonly IPurchaseRepository _purchaseRepo;

        public GetVATReportQueryHandler(
            ISaleRepository saleRepo,
            IPurchaseRepository purchaseRepo)
        {
            _saleRepo = saleRepo;
            _purchaseRepo = purchaseRepo;
        }

        public async Task<VATReportDto> Handle(
            GetVATReportQuery request,
            CancellationToken cancellationToken)
        {
            // ── Step 1: Get all sales and purchases ───────────────
            var allSales = await _saleRepo
                .GetByDateRangeAsync(
                    request.FromDate, request.ToDate);

            var allPurchases = await _purchaseRepo
                .GetByDateRangeAsync(
                    request.FromDate, request.ToDate);

            // ── Step 2: Filter only VAT transactions ──────────────
            // Only include transactions where VAT was applied
            var vatSales = allSales
                .Where(s => s.VATAmount > 0)
                .ToList();

            var vatPurchases = allPurchases
                .Where(p => p.VATAmount > 0)
                .ToList();

            // ── Step 3: Build Output VAT (from sales) ─────────────
            var outputTransactions = vatSales
                .Select(s => new VATTransactionDto
                {
                    Date = s.SaleDate,
                    ReferenceNumber = s.SaleNumber,

                    // Retailer who bought from us
                    PartyName = s.Customer?.Name ?? "Unknown",
                    PartyPAN = s.Customer?.PANNumber,

                    // Amount before VAT
                    TaxableAmount = s.SubTotal - s.DiscountAmount,
                    VATPercent = s.VATPercent,
                    VATAmount = s.VATAmount,
                    TotalAmount = s.TotalAmount
                })
                // Newest first
                .OrderByDescending(t => t.Date)
                .ToList();

            var outputVAT = new OutputVATDto
            {
                Transactions = outputTransactions,
                TotalTaxableAmount = outputTransactions
                    .Sum(t => t.TaxableAmount),
                TotalVATAmount = outputTransactions
                    .Sum(t => t.VATAmount),
                TotalAmount = outputTransactions
                    .Sum(t => t.TotalAmount),
                TransactionCount = outputTransactions.Count
            };

            // ── Step 4: Build Input VAT (from purchases) ──────────
            var inputTransactions = vatPurchases
                .Select(p => new VATTransactionDto
                {
                    Date = p.PurchaseDate,
                    ReferenceNumber = p.PurchaseNumber,

                    // Supplier we bought from
                    PartyName = p.Supplier?.Name ?? "Unknown",
                    PartyPAN = p.Supplier?.PANNumber,

                    // Amount before VAT
                    TaxableAmount = p.SubTotal,
                    VATPercent = p.VATPercent,
                    VATAmount = p.VATAmount,
                    TotalAmount = p.TotalAmount
                })
                .OrderByDescending(t => t.Date)
                .ToList();

            var inputVAT = new InputVATDto
            {
                Transactions = inputTransactions,
                TotalTaxableAmount = inputTransactions
                    .Sum(t => t.TaxableAmount),
                TotalVATAmount = inputTransactions
                    .Sum(t => t.VATAmount),
                TotalAmount = inputTransactions
                    .Sum(t => t.TotalAmount),
                TransactionCount = inputTransactions.Count
            };

            // ── Step 5: Calculate VAT payable ─────────────────────

            // Output VAT = what you collected from retailers
            decimal outputVATTotal = outputVAT.TotalVATAmount;

            // Input VAT = what you paid to suppliers
            decimal inputVATTotal = inputVAT.TotalVATAmount;

            // VAT payable = what you must send to IRD
            decimal vatPayable = outputVATTotal - inputVATTotal;

            // Build a clear status message for the owner
            string vatStatus;
            if (vatPayable > 0)
                vatStatus =
                    $"You must pay Rs. {vatPayable:N2} to IRD";
            else if (vatPayable < 0)
                vatStatus =
                    $"IRD owes you Rs. {Math.Abs(vatPayable):N2} " +
                    $"(carry forward to next month)";
            else
                vatStatus = "No VAT payable this period";

            var summary = new VATSummaryDto
            {
                OutputVAT = outputVATTotal,
                InputVAT = inputVATTotal,
                VATPayable = vatPayable,
                VATStatus = vatStatus
            };

            // ── Step 6: Return full report ────────────────────────
            return new VATReportDto
            {
                FromDate = request.FromDate,
                ToDate = request.ToDate,
                GeneratedAt = DateTime.UtcNow,
                VATRate = 13,
                OutputVAT = outputVAT,
                InputVAT = inputVAT,
                Summary = summary
            };
        }
    }
}