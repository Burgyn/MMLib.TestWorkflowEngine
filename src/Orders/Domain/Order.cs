namespace Orders.Domain;

public class Order
{
    public int Id { get; set; }

    public required string CustomerName { get; set; }

    public required string Description { get; set; }

    public decimal TotalAmount { get; set; }

    public OrderStatus Status { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? LastModifiedAt { get; set; }
}