using MediatR;
using Microsoft.AspNetCore.Mvc;
using PdfCreator.Domain.Models;

namespace PdfCreator.Features.GenerateInvoicePdf;

[ApiController]
[Route("api/invoices")]
public class GenerateInvoicePdfEndpoint : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<GenerateInvoicePdfEndpoint> _logger;

    public GenerateInvoicePdfEndpoint(
        IMediator mediator,
        ILogger<GenerateInvoicePdfEndpoint> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpPost("generate-pdf")]
    public async Task<IActionResult> GeneratePdf([FromBody] Invoice invoice, CancellationToken cancellationToken)
    {
        try
        {
            var pdfBytes = await _mediator.Send(new GenerateInvoicePdfCommand(invoice), cancellationToken);
            return File(pdfBytes, "application/pdf", $"invoice-{invoice.Number}.pdf");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating PDF for invoice {InvoiceNumber}", invoice.Number);
            return StatusCode(500, "Error generating PDF");
        }
    }
} 