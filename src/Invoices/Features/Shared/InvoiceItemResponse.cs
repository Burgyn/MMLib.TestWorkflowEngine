namespace Invoices.Features.Shared;

public record InvoiceItemResponse(
    int Id,
    string Description,
    decimal UnitPrice,
    int Quantity,
    decimal TotalAmount); 