using Invoices.Domain;
using Invoices.Features.Shared;

namespace Invoices.Features.GetInvoices;

public record GetInvoicesResponse(
    int Id,
    string Number,
    string CustomerName,
    decimal TotalAmount,
    DateTime IssueDate,
    DateTime DueDate,
    string? PaymentReference,
    InvoiceStatus Status,
    DateTime CreatedAt,
    DateTime? PaidAt,
    IReadOnlyList<InvoiceItemResponse> Items); 