using Microsoft.AspNetCore.Mvc;
using PdfCreator.Documents;
using PdfCreator.Models;
using QuestPDF.Fluent;

namespace PdfCreator.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InvoiceController : ControllerBase
{
    private readonly ILogger<InvoiceController> _logger;

    public InvoiceController(ILogger<InvoiceController> logger)
    {
        _logger = logger;
    }

    [HttpPost("generate")]
    public IActionResult GeneratePdf([FromBody] Invoice invoice)
    {
        try
        {
            var document = new InvoiceDocument(invoice);
            var pdfBytes = document.GeneratePdf();

            return File(pdfBytes, "application/pdf", $"invoice-{invoice.InvoiceNumber}.pdf");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating PDF for invoice {InvoiceNumber}", invoice.InvoiceNumber);
            return StatusCode(500, "Error generating PDF");
        }
    }
} 