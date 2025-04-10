namespace Invoices.Domain.Events;

public record InvoicePaidEvent(
    int Id,
    string Number,
    string CustomerName,
    decimal TotalAmount,
    string PaymentReference,
    DateTime PaidAt); 