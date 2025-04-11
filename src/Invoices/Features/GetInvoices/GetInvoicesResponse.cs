using Invoices.Domain;
using Invoices.Features.Shared;

namespace Invoices.Features.GetInvoices;

public record GetInvoicesResponse(
    int Id,
    string Number,
    string CustomerName,
    decimal TotalAmount,
    decimal PaidAmount,
    decimal RemainingAmount,
    DateTime IssueDate,
    DateTime DueDate,
    string? PaymentReference,
    int? OrderId,
    InvoiceStatus Status,
    DateTime CreatedAt,
    DateTime? PaidAt,
    IReadOnlyList<InvoiceItemResponse> Items,
    IReadOnlyList<InvoicePaymentResponse> Payments); 