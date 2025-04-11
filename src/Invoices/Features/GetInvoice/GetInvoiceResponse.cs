using Invoices.Domain;
using Invoices.Features.Shared;

namespace Invoices.Features.GetInvoice;

public record GetInvoiceResponse(
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
    IEnumerable<InvoiceItemResponse> Items,
    IEnumerable<InvoicePaymentResponse> Payments); 