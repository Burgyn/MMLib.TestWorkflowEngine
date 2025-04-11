using PdfCreator.Domain.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace PdfCreator.Infrastructure.PdfGeneration;

public class QuestPdfInvoiceGenerator : IInvoicePdfGenerator
{
    private readonly ILogger<QuestPdfInvoiceGenerator> _logger;
    private readonly IWebHostEnvironment _environment;
    private static TextStyle DefaultTextStyle => TextStyle.Default.FontFamily("Arial");

    public QuestPdfInvoiceGenerator(
        ILogger<QuestPdfInvoiceGenerator> logger,
        IWebHostEnvironment environment)
    {
        _logger = logger;
        _environment = environment;
    }

    public Task<byte[]> GenerateAsync(Invoice invoice, CancellationToken cancellationToken)
    {
        var document = new InvoiceDocument(invoice, _environment, DefaultTextStyle);
        return Task.FromResult(document.GeneratePdf());
    }

    private class InvoiceDocument : IDocument
    {
        private readonly Invoice _invoice;
        private readonly string _logoPath;
        private readonly TextStyle _textStyle;

        public InvoiceDocument(Invoice invoice, IWebHostEnvironment environment, TextStyle textStyle)
        {
            _invoice = invoice;
            _logoPath = Path.Combine(environment.WebRootPath, "images", "logo.png");
            _textStyle = textStyle;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            container
                .Page(page =>
                {
                    page.Margin(50);
                    page.DefaultTextStyle(_textStyle);
                    
                    page.Header().Element(ComposeHeader);
                    page.Content().Element(ComposeContent);
                    page.Footer().Element(ComposeFooter);
                });
        }

        private void ComposeHeader(IContainer container)
        {
            container.Row(row =>
            {
                if (File.Exists(_logoPath))
                {
                    row.ConstantItem(100).Height(50).Image(_logoPath);
                    row.AutoItem().Width(50);
                }
                
                row.RelativeItem().Column(column =>
                {
                    column.Item().Text("FAKTÚRA").FontSize(20).Bold();
                    column.Item().Text($"Číslo: {_invoice.Number}").FontSize(12);
                    column.Item().Text($"Dátum vystavenia: {_invoice.IssueDate:d.M.yyyy}").FontSize(12);
                    column.Item().Text($"Dátum splatnosti: {_invoice.DueDate:d.M.yyyy}").FontSize(12);
                    if (!string.IsNullOrEmpty(_invoice.PaymentReference))
                    {
                        column.Item().Text($"Variabilný symbol: {_invoice.PaymentReference}").FontSize(12);
                    }
                });
            });
        }

        private void ComposeContent(IContainer container)
        {
            container.PaddingVertical(20).Column(column =>
            {
                // Customer information
                column.Item().Column(customerInfo =>
                {
                    customerInfo.Item().Text("Odberateľ").Bold();
                    customerInfo.Item().PaddingTop(5).Text(_invoice.CustomerName);
                });

                // Items
                column.Item().PaddingTop(25).Element(ComposeTable);

                // Summary
                column.Item().AlignRight().PaddingTop(25).Text($"Celková suma: {_invoice.TotalAmount:N2} EUR").Bold();

                // Additional information
                if (_invoice.OrderId.HasValue)
                {
                    column.Item().PaddingTop(10).Text($"Objednávka č.: {_invoice.OrderId}");
                }

                column.Item().PaddingTop(10).Text($"Stav: {_invoice.Status}");
                column.Item().Text($"Vytvorené: {_invoice.CreatedAt:d.M.yyyy}");
                if (_invoice.PaidAt.HasValue)
                {
                    column.Item().Text($"Zaplatené: {_invoice.PaidAt.Value:d.M.yyyy}");
                }
            });
        }

        private void ComposeTable(IContainer container)
        {
            container.Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.ConstantColumn(25);
                    columns.RelativeColumn(3);
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                });

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

                foreach (var (item, index) in _invoice.Items.Select((item, index) => (item, index)))
                {
                    table.Cell().Text($"{index + 1}");
                    table.Cell().Text(item.Description);
                    table.Cell().AlignRight().Text(item.Unit);
                    table.Cell().AlignRight().Text($"{item.Quantity:N2}");
                    table.Cell().AlignRight().Text($"{item.UnitPrice:N2}");
                    table.Cell().AlignRight().Text($"{item.TotalAmount:N2}");
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

    private class AddressComponent : IComponent
    {
        private readonly string _title;
        private readonly Company _company;

        public AddressComponent(string title, Company company)
        {
            _title = title;
            _company = company;
        }

        public void Compose(IContainer container)
        {
            container.ShowEntire().Column(column =>
            {
                column.Item().Text(_title).Bold();
                column.Item().PaddingTop(5).Text(_company.Name);
                column.Item().Text(_company.StreetAddress);
                column.Item().Text($"{_company.ZipCode} {_company.City}");
                column.Item().Text(_company.Country);
                
                if (!string.IsNullOrEmpty(_company.VatNumber))
                {
                    column.Item().PaddingTop(5).Text($"IČ DPH: {_company.VatNumber}");
                }
            });
        }
    }
} 