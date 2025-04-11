namespace Invoices.Features.PayInvoice;

public record PayInvoiceRequest(
    decimal Amount,
    string PaymentReference); 