namespace Invoices.Domain;

public class InvoiceItem
{
    public int Id { get; set; }

    public int InvoiceId { get; set; }

    public required string Description { get; set; }

    public required decimal UnitPrice { get; set; }

    public required int Quantity { get; set; }

    public decimal TotalAmount => UnitPrice * Quantity;

    public Invoice? Invoice { get; set; }
} 