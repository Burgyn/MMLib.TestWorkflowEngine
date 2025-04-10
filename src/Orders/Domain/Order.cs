namespace Orders.Domain;

public class Order
{
    public int Id { get; set; }

    public required string CustomerName { get; set; }

    public required string Description { get; set; }

    public required decimal UnitPrice { get; set; }

    public required int Quantity { get; set; }

    public decimal TotalAmount => UnitPrice * Quantity;

    public OrderStatus Status { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? LastModifiedAt { get; set; }
}