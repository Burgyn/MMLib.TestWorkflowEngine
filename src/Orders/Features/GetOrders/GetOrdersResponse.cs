using Orders.Domain;

namespace Orders.Features.GetOrders;

public record GetOrdersResponse(
    int Id,
    string CustomerName,
    string Description,
    decimal UnitPrice,
    int Quantity,
    decimal TotalAmount,
    OrderStatus Status,
    DateTime CreatedAt,
    DateTime? LastModifiedAt); 