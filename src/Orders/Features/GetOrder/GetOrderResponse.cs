using Orders.Domain;

namespace Orders.Features.GetOrder;

public record GetOrderResponse(
    int Id,
    string CustomerName,
    string Description,
    decimal TotalAmount,
    OrderStatus Status,
    DateTime CreatedAt,
    DateTime? LastModifiedAt); 