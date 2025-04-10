using Orders.Domain;
using Orders.Infrastructure;

namespace Orders.Features.CreateOrder;

public static class CreateOrderEndpoint
{
    public static IEndpointRouteBuilder MapCreateOrder(this IEndpointRouteBuilder app)
    {
        app.MapPost("/orders", async (OrderDbContext dbContext, CreateOrderRequest request) =>
        {
            var order = new Order
            {
                CustomerName = request.CustomerName,
                Description = request.Description,
                TotalAmount = request.TotalAmount,
                Status = OrderStatus.Created,
                CreatedAt = DateTime.UtcNow
            };

            dbContext.Orders.Add(order);
            await dbContext.SaveChangesAsync();

            return Results.Created($"/orders/{order.Id}", order.Id);
        })
        .WithName("CreateOrder")
        .WithOpenApi();

        return app;
    }
} 