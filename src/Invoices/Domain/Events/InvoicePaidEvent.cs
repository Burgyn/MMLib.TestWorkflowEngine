namespace Invoices.Domain.Events;

public record InvoicePaidEvent(
    int Id,
    string Number,
    string CustomerName,
    decimal TotalAmount,
    decimal TotalPaidAmount,
    decimal PaymentAmount,
    string PaymentReference,
    DateTime PaidAt); 