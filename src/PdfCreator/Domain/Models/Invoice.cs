namespace PdfCreator.Domain.Models;

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

    public List<InvoiceItem> Items { get; set; } = new();

    public decimal TotalAmount => Items.Sum(x => x.TotalAmount);
}

public class Company
{
    public string Name { get; set; } = string.Empty;
    public string StreetAddress { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string VatNumber { get; set; } = string.Empty;
}

public class InvoiceItem
{
    public string Description { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalAmount => Quantity * UnitPrice;
    public string Unit { get; set; } = "ks";
}

public enum InvoiceStatus
{
    Created,
    Paid,
    Cancelled
} 