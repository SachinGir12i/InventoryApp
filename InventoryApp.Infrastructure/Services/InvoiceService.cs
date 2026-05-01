using InventoryApp.Application.Features.Invoices.DTOs;
using InventoryApp.Application.Interfaces;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace InventoryApp.Infrastructure.Services
{
    public class InvoiceService : IInvoiceService
    {
        public InvoiceService()
        {
            // Set QuestPDF license
            // Community license is free for small projects
            QuestPDF.Settings.License = LicenseType.Community;
        }

        public byte[] GenerateInvoicePdf(InvoiceDto invoice)
        {
            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    // ── Page setup ───────────────────────────────
                    page.Size(PageSizes.A4);
                    page.Margin(30);
                    page.DefaultTextStyle(x =>
                        x.FontSize(10).FontFamily("Arial"));

                    // ── Header ───────────────────────────────────
                    page.Header().Element(ComposeHeader);

                    // ── Content ──────────────────────────────────
                    page.Content().Element(content =>
                        ComposeContent(content, invoice));

                    // ── Footer ───────────────────────────────────
                    page.Footer().Element(ComposeFooter);

                    // ── Local functions ──────────────────────────
                    void ComposeHeader(IContainer container)
                    {
                        container.Column(col =>
                        {
                            // Company name at top
                            col.Item()
                                .Text(invoice.Company.Name)
                                .Bold()
                                .FontSize(18);

                            col.Item()
                                .Text(invoice.Company.Address ?? "")
                                .FontSize(9)
                                .FontColor(Colors.Grey.Medium);

                            if (!string.IsNullOrEmpty(
                                invoice.Company.Phone))
                                col.Item()
                                    .Text($"Phone: {invoice.Company.Phone}")
                                    .FontSize(9);

                            if (!string.IsNullOrEmpty(
                                invoice.Company.PANNumber))
                                col.Item()
                                    .Text($"PAN: {invoice.Company.PANNumber}")
                                    .FontSize(9);

                            // Horizontal line
                            col.Item()
                                .PaddingTop(5)
                                .LineHorizontal(1)
                                .LineColor(Colors.Grey.Medium);
                        });
                    }

                    void ComposeContent(
                        IContainer container, InvoiceDto inv)
                    {
                        container.Column(col =>
                        {
                            col.Spacing(10);

                            // ── Invoice title and number ──────────
                            col.Item().Row(row =>
                            {
                                // Left: "TAX INVOICE" title
                                row.RelativeItem()
                                    .Text("TAX INVOICE")
                                    .Bold()
                                    .FontSize(16);

                                // Right: Invoice number and date
                                row.RelativeItem()
                                    .AlignRight()
                                    .Column(c =>
                                    {
                                        c.Item()
                                            .Text($"Invoice #: " +
                                                $"{inv.InvoiceNumber}")
                                            .Bold();
                                        c.Item()
                                            .Text($"Date: " +
                                                $"{inv.InvoiceDate:dd/MM/yyyy}");
                                        c.Item()
                                            .Text($"Status: " +
                                                $"{inv.PaymentStatus}")
                                            .FontColor(
                                                inv.PaymentStatus == "FullyPaid"
                                                ? Colors.Green.Medium
                                                : Colors.Red.Medium);
                                    });
                            });

                            // ── Bill To section ───────────────────
                            col.Item()
                                .Background(Colors.Grey.Lighten3)
                                .Padding(8)
                                .Column(c =>
                                {
                                    c.Item()
                                        .Text("Bill To:")
                                        .Bold()
                                        .FontSize(9)
                                        .FontColor(Colors.Grey.Medium);

                                    c.Item()
                                        .Text(inv.CustomerName)
                                        .Bold()
                                        .FontSize(11);

                                    if (!string.IsNullOrEmpty(
                                        inv.CustomerAddress))
                                        c.Item()
                                            .Text(inv.CustomerAddress);

                                    if (!string.IsNullOrEmpty(
                                        inv.CustomerPhone))
                                        c.Item()
                                            .Text($"Phone: {inv.CustomerPhone}");

                                    if (!string.IsNullOrEmpty(
                                        inv.CustomerPAN))
                                        c.Item()
                                            .Text($"PAN: {inv.CustomerPAN}");
                                });

                            // ── Items table ───────────────────────
                            col.Item().Table(table =>
                            {
                                // Define columns
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.ConstantColumn(25);  // S.No
                                    columns.RelativeColumn(4);   // Item
                                    columns.RelativeColumn(1.5f);// Qty
                                    columns.RelativeColumn(2);   // Rate
                                    columns.RelativeColumn(2);   // Disc
                                    columns.RelativeColumn(2);   // Amount
                                });

                                // Table header
                                static IContainer HeaderCell(
                                    IContainer container)
                                    => container
                                        .Background(Colors.Grey.Darken2)
                                        .Padding(5);

                                table.Header(header =>
                                {
                                    header.Cell().Element(HeaderCell)
                                        .Text("#")
                                        .FontColor(Colors.White)
                                        .Bold();

                                    header.Cell().Element(HeaderCell)
                                        .Text("Item")
                                        .FontColor(Colors.White)
                                        .Bold();

                                    header.Cell().Element(HeaderCell)
                                        .AlignRight()
                                        .Text("Qty")
                                        .FontColor(Colors.White)
                                        .Bold();

                                    header.Cell().Element(HeaderCell)
                                        .AlignRight()
                                        .Text("Rate")
                                        .FontColor(Colors.White)
                                        .Bold();

                                    header.Cell().Element(HeaderCell)
                                        .AlignRight()
                                        .Text("Disc")
                                        .FontColor(Colors.White)
                                        .Bold();

                                    header.Cell().Element(HeaderCell)
                                        .AlignRight()
                                        .Text("Amount")
                                        .FontColor(Colors.White)
                                        .Bold();
                                });

                                // Table rows — alternating background
                                bool isAlternate = false;
                                foreach (var item in inv.LineItems)
                                {
                                    var bgColor = isAlternate
                                        ? Colors.Grey.Lighten4
                                        : Colors.White;
                                    isAlternate = !isAlternate;

                                    IContainer DataCell(
                                        IContainer c) =>
                                        c.Background(bgColor)
                                         .Padding(5);

                                    table.Cell()
                                        .Element(DataCell)
                                        .Text(item.SNo.ToString());

                                    table.Cell()
                                        .Element(DataCell)
                                        .Text(item.ItemName);

                                    table.Cell()
                                        .Element(DataCell)
                                        .AlignRight()
                                        .Text(item.Quantity
                                            .ToString("N0"));

                                    table.Cell()
                                        .Element(DataCell)
                                        .AlignRight()
                                        .Text($"Rs.{item.UnitPrice:N2}");

                                    table.Cell()
                                        .Element(DataCell)
                                        .AlignRight()
                                        .Text(item.Discount > 0
                                            ? $"Rs.{item.Discount:N2}"
                                            : "-");

                                    table.Cell()
                                        .Element(DataCell)
                                        .AlignRight()
                                        .Text($"Rs.{item.Amount:N2}");
                                }
                            });

                            // ── Totals section ────────────────────
                            col.Item().AlignRight().Column(totals =>
                            {
                                totals.Spacing(3);

                                // Subtotal row
                                AddTotalRow(totals,
                                    "Subtotal:",
                                    $"Rs.{inv.SubTotal:N2}",
                                    false);

                                // Discount row (only show if > 0)
                                if (inv.DiscountAmount > 0)
                                    AddTotalRow(totals,
                                        "Discount:",
                                        $"-Rs.{inv.DiscountAmount:N2}",
                                        false,
                                        Colors.Red.Medium);

                                // VAT row (only show if > 0)
                                if (inv.VATAmount > 0)
                                    AddTotalRow(totals,
                                        $"VAT ({inv.VATPercent}%):",
                                        $"Rs.{inv.VATAmount:N2}",
                                        false);

                                // Divider line before total
                                totals.Item()
                                    .Width(200)
                                    .LineHorizontal(0.5f)
                                    .LineColor(Colors.Grey.Medium);

                                // Grand total — bold and larger
                                AddTotalRow(totals,
                                    "Total:",
                                    $"Rs.{inv.TotalAmount:N2}",
                                    true);

                                // Received amount
                                if (inv.ReceivedAmount > 0)
                                    AddTotalRow(totals,
                                        "Received:",
                                        $"-Rs.{inv.ReceivedAmount:N2}",
                                        false,
                                        Colors.Green.Medium);

                                // Due amount (only show if > 0)
                                if (inv.DueAmount > 0)
                                    AddTotalRow(totals,
                                        "Due:",
                                        $"Rs.{inv.DueAmount:N2}",
                                        true,
                                        Colors.Red.Medium);
                            });

                            // ── Amount in words ───────────────────
                            col.Item()
                                .Background(Colors.Grey.Lighten3)
                                .Padding(8)
                                .Text($"In Words: {inv.AmountInWords} Rupees Only")
                                .Italic()
                                .FontSize(9);

                            // ── Remarks ───────────────────────────
                            if (!string.IsNullOrEmpty(inv.Remarks))
                                col.Item()
                                    .Column(c =>
                                    {
                                        c.Item()
                                            .Text("Remarks:")
                                            .Bold()
                                            .FontSize(9);
                                        c.Item()
                                            .Text(inv.Remarks)
                                            .FontSize(9);
                                    });

                            // ── Signature section ─────────────────
                            col.Item()
                                .PaddingTop(20)
                                .Row(row =>
                                {
                                    // Customer signature
                                    row.RelativeItem()
                                        .Column(c =>
                                        {
                                            c.Item()
                                                .PaddingTop(30)
                                                .LineHorizontal(0.5f)
                                                .LineColor(Colors.Grey.Medium);
                                            c.Item()
                                                .Text("Customer Signature")
                                                .FontSize(9)
                                                .FontColor(Colors.Grey.Medium);
                                        });

                                    row.ConstantItem(50);

                                    // Authorized signature
                                    row.RelativeItem()
                                        .Column(c =>
                                        {
                                            c.Item()
                                                .PaddingTop(30)
                                                .LineHorizontal(0.5f)
                                                .LineColor(Colors.Grey.Medium);
                                            c.Item()
                                                .Text("Authorized Signature")
                                                .FontSize(9)
                                                .FontColor(Colors.Grey.Medium);
                                        });
                                });
                        });
                    }

                    void ComposeFooter(IContainer container)
                    {
                        container.Column(col =>
                        {
                            // Divider line
                            col.Item()
                                .LineHorizontal(0.5f)
                                .LineColor(Colors.Grey.Lighten2);

                            col.Item()
                                .PaddingTop(5)
                                .Row(row =>
                                {
                                    // Thank you message
                                    row.RelativeItem()
                                        .Text("Thank you for your business!")
                                        .FontSize(8)
                                        .FontColor(Colors.Grey.Medium)
                                        .Italic();

                                    // Page number
                                    row.RelativeItem()
                                        .AlignRight()
                                        .Text(text =>
                                        {
                                            text.Span("Page ")
                                                .FontSize(8)
                                                .FontColor(Colors.Grey.Medium);
                                            text.CurrentPageNumber()
                                                .FontSize(8)
                                                .FontColor(Colors.Grey.Medium);
                                            text.Span(" of ")
                                                .FontSize(8)
                                                .FontColor(Colors.Grey.Medium);
                                            text.TotalPages()
                                                .FontSize(8)
                                                .FontColor(Colors.Grey.Medium);
                                        });
                                });
                        });
                    }
                });
            }).GeneratePdf();
        }

        // ── Helper to add a total row ────────────────────────────
        private static void AddTotalRow(
            ColumnDescriptor col,
            string label,
            string value,
            bool isBold,
            string? color = null)
        {
            col.Item().Row(row =>
            {
                // Label
                var labelText = row.ConstantItem(100)
                    .AlignRight()
                    .Text(label);
                if (isBold) labelText.Bold();

                row.ConstantItem(10);

                // Value
                var valueText = row.ConstantItem(100)
                    .AlignRight()
                    .Text(value);
                if (isBold) valueText.Bold();
                if (color != null) valueText.FontColor(color);
            });
        }
    }
}