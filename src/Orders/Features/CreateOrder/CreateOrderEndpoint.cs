using Microsoft.AspNetCore.Http.HttpResults;
using Orders.Domain;
using Orders.Infrastructure;

namespace Orders.Features.CreateOrder;

public static class CreateOrderEndpoint
{
    public static IEndpointRouteBuilder MapCreateOrder(this IEndpointRouteBuilder app)
    {
        app.MapPost("/orders", CreateOrder)
           .WithName("CreateOrder")
           .WithOpenApi();

        return app;
    }

    private static async Task<Created<int>> CreateOrder(OrderDbContext dbContext, CreateOrderRequest request)
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

        return TypedResults.Created($"/orders/{order.Id}", order.Id);
    }
} 