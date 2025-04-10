namespace Invoices.Domain;

public class Invoice
{
    public int Id { get; set; }

    public required string Number { get; set; }

    public required string CustomerName { get; set; }

    public required DateTime IssueDate { get; set; }

    public required DateTime DueDate { get; set; }

    public string? PaymentReference { get; set; }

    public InvoiceStatus Status { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? PaidAt { get; set; }

    public List<InvoiceItem> Items { get; set; } = new();

    public decimal TotalAmount => Items.Sum(x => x.TotalAmount);
} 