namespace Invoices.Domain;

public class Invoice
{
    public int Id { get; set; }

    public required string Number { get; set; }

    public required string CustomerName { get; set; }

    public required DateTime IssueDate { get; set; }

    public required DateTime DueDate { get; set; }

    public string? PaymentReference { get; set; }

    public int? OrderId { get; set; }

    public InvoiceStatus Status { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? PaidAt { get; set; }

    public decimal PaidAmount { get; set; }

    public List<InvoicePayment> Payments { get; set; } = new();

    public List<InvoiceItem> Items { get; set; } = new();

    public decimal TotalAmount => Items.Sum(x => x.TotalAmount);

    public decimal RemainingAmount => TotalAmount - PaidAmount;

    public void AddPayment(decimal amount, string reference)
    {
        var payment = new InvoicePayment
        {
            Amount = amount,
            PaymentReference = reference,
            PaidAt = DateTime.UtcNow
        };

        Payments.Add(payment);
        PaidAmount += amount;
        UpdateStatus();
    }

    private void UpdateStatus()
    {
        Status = PaidAmount switch
        {
            0 => InvoiceStatus.Unpaid,
            > 0 when PaidAmount < TotalAmount => InvoiceStatus.PartiallyPaid,
            _ when PaidAmount == TotalAmount => InvoiceStatus.Paid,
            _ => InvoiceStatus.Overpaid
        };

        if (Status is InvoiceStatus.Paid or InvoiceStatus.Overpaid)
        {
            PaidAt = DateTime.UtcNow;
        }
    }
} 