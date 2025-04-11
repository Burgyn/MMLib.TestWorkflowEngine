using PdfCreator.Domain.Models;

namespace PdfCreator.Infrastructure.PdfGeneration;

public interface IInvoicePdfGenerator
{
    Task<byte[]> GenerateAsync(Invoice invoice, CancellationToken cancellationToken);
} 