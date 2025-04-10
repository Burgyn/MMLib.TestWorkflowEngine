namespace Orders.Features.CreateOrder;

public record CreateOrderRequest(
    string CustomerName,
    string Description,
    decimal TotalAmount); 