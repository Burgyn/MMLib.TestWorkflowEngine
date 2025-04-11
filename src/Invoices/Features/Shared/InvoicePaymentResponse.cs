namespace Invoices.Features.Shared;

public record InvoicePaymentResponse(
    int Id,
    decimal Amount,
    string PaymentReference,
    DateTime PaidAt); 