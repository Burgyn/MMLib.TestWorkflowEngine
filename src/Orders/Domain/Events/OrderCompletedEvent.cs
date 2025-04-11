using Orders.Domain;

namespace Orders.Domain.Events;

public record OrderCompletedEvent(
    int Id,
    string CustomerName,
    string Description,
    decimal TotalAmount,
    OrderStatus Status,
    DateTime CreatedAt,
    DateTime CompletedAt); 