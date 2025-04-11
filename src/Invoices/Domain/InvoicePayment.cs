namespace Invoices.Domain;

public class InvoicePayment
{
    public int Id { get; set; }

    public required decimal Amount { get; set; }

    public required string PaymentReference { get; set; }

    public required DateTime PaidAt { get; set; }

    public int InvoiceId { get; set; }

    public Invoice Invoice { get; set; } = null!;
} 