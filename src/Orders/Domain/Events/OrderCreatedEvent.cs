namespace Orders.Domain.Events;

public record OrderCreatedEvent(
    int Id,
    string CustomerName,
    string Description,
    decimal TotalAmount,
    OrderStatus Status,
    DateTime CreatedAt); 