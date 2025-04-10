namespace Orders.Features.CreateOrder;

public record CreateOrderRequest(
    string CustomerName,
    string Description,
    decimal UnitPrice,
    int Quantity); 