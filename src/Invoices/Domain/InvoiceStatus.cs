namespace Invoices.Domain;

public enum InvoiceStatus
{
    Created,
    Sent,
    Unpaid,
    PartiallyPaid,
    Paid,
    Overpaid
} 