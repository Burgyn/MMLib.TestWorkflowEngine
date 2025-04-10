using Orders.Domain;

namespace Orders.Features.UpdateOrderStatus;

public record UpdateOrderStatusRequest(OrderStatus NewStatus); 