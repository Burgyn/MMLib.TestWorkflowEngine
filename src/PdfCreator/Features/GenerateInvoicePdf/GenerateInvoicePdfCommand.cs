using MediatR;
using PdfCreator.Domain.Models;
using PdfCreator.Infrastructure.PdfGeneration;

namespace PdfCreator.Features.GenerateInvoicePdf;

public record GenerateInvoicePdfCommand(Invoice Invoice) : IRequest<byte[]>;

public class GenerateInvoicePdfCommandHandler : IRequestHandler<GenerateInvoicePdfCommand, byte[]>
{
    private readonly ILogger<GenerateInvoicePdfCommandHandler> _logger;
    private readonly IInvoicePdfGenerator _pdfGenerator;

    public GenerateInvoicePdfCommandHandler(
        ILogger<GenerateInvoicePdfCommandHandler> logger,
        IInvoicePdfGenerator pdfGenerator)
    {
        _logger = logger;
        _pdfGenerator = pdfGenerator;
    }

    public async Task<byte[]> Handle(GenerateInvoicePdfCommand request, CancellationToken cancellationToken)
    {
        try
        {
            return await _pdfGenerator.GenerateAsync(request.Invoice, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating PDF for invoice {InvoiceNumber}", request.Invoice.Number);
            throw;
        }
    }
}