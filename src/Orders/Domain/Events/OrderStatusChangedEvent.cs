namespace Orders.Domain.Events;

public record OrderStatusChangedEvent(
    int Id,
    string CustomerName,
    string Description,
    decimal TotalAmount,
    OrderStatus Status,
    DateTime CreatedAt,
    DateTime LastModifiedAt); 