namespace Invoices.Domain.Events;

public record InvoiceCreatedEvent(
    int Id,
    string Number,
    string CustomerName,
    decimal TotalAmount,
    DateTime IssueDate,
    DateTime DueDate,
    InvoiceStatus Status,
    DateTime CreatedAt);