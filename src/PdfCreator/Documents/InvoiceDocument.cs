using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using PdfCreator.Models;

namespace PdfCreator.Documents;

public class InvoiceDocument : IDocument
{
    private readonly Invoice _invoice;
    private static readonly string LogoPath = "wwwroot/images/logo.png";

    public InvoiceDocument(Invoice invoice)
    {
        _invoice = invoice;
    }

    public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

    public void Compose(IDocumentContainer container)
    {
        container
            .Page(page =>
            {
                page.Margin(50);

                page.Header().Element(ComposeHeader);
                page.Content().Element(ComposeContent);
                page.Footer().Element(ComposeFooter);
            });
    }

    private void ComposeHeader(IContainer container)
    {
        container.Row(row =>
        {
            // Logo and company name
            row.ConstantItem(100).Height(50).Image(LogoPath);

            row.AutoItem().Width(50);

            row.RelativeItem().Column(column =>
            {
                column.Item().Text("FAKTÚRA").FontSize(20).Bold();
                column.Item().Text($"Číslo: {_invoice.InvoiceNumber}").FontSize(12);
                column.Item().Text($"Dátum vystavenia: {_invoice.IssueDate:d.M.yyyy}").FontSize(12);
                column.Item().Text($"Dátum splatnosti: {_invoice.DueDate:d.M.yyyy}").FontSize(12);
            });
        });
    }

    private void ComposeContent(IContainer container)
    {
        container.PaddingVertical(20).Column(column =>
        {
            // Supplier and customer information
            column.Item().Row(row =>
            {
                row.ConstantItem(50);
            });

            // Items
            column.Item().PaddingTop(25).Element(ComposeTable);

            // Summary
            column.Item().AlignRight().PaddingTop(25).Text($"Celková suma: {_invoice.TotalAmount:N2} {_invoice.Currency}").Bold();
        });
    }

    private void ComposeTable(IContainer container)
    {
        container.Table(table =>
        {
            // Step 1: define columns
            table.ColumnsDefinition(columns =>
            {
                columns.ConstantColumn(25);    // Index
                columns.RelativeColumn(3);     // Description
                columns.RelativeColumn();      // Unit
                columns.RelativeColumn();      // Quantity
                columns.RelativeColumn();      // Unit price
                columns.RelativeColumn();      // Total price
            });

            // Step 2: create header
            table.Header(header =>
            {
                header.Cell().Text("#");
                header.Cell().Text("Popis");
                header.Cell().AlignRight().Text("Jednotka");
                header.Cell().AlignRight().Text("Množstvo");
                header.Cell().AlignRight().Text("Cena za j.");
                header.Cell().AlignRight().Text("Spolu");

                header.Cell().ColumnSpan(6).PaddingTop(5).BorderBottom(1).BorderColor(Colors.Black);
            });

            // Step 3: compose content
            foreach (var (item, index) in _invoice.Items.Select((item, index) => (item, index)))
            {
                table.Cell().Text($"{index + 1}");
                table.Cell().Text(item.Description);
                table.Cell().AlignRight().Text(item.Unit);
                table.Cell().AlignRight().Text($"{item.Quantity:N2}");
                table.Cell().AlignRight().Text($"{item.UnitPrice:N2}");
                table.Cell().AlignRight().Text($"{item.TotalPrice:N2}");
            }
        });
    }

    private void ComposeFooter(IContainer container)
    {
        container.AlignCenter().Text(text =>
        {
            text.Span("Strana ").FontSize(10);
            text.CurrentPageNumber().FontSize(10);
            text.Span(" z ").FontSize(10);
            text.TotalPages().FontSize(10);
        });
    }
}