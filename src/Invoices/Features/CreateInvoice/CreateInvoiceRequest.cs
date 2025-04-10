namespace Invoices.Features.CreateInvoice;

public record CreateInvoiceRequest(
    string CustomerName,
    DateTime DueDate,
    IReadOnlyList<CreateInvoiceItemRequest> Items);

public record CreateInvoiceItemRequest(
    string Description,
    decimal UnitPrice,
    int Quantity); 