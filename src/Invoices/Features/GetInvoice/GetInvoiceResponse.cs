using Invoices.Domain;
using Invoices.Features.Shared;

namespace Invoices.Features.GetInvoice;

public record GetInvoiceResponse(
    int Id,
    string Number,
    string CustomerName,
    DateTime CreatedAt,
    decimal TotalAmount,
    IEnumerable<InvoiceItemResponse> Items); 