using Microsoft.AspNetCore.Http.HttpResults;
using Orders.Domain;
using Orders.Domain.Events;
using Orders.Infrastructure;
using Orders.Infrastructure.DomainEvents;

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

    private static async Task<Created<int>> CreateOrder(
        OrderDbContext dbContext,
        IEventPublisher eventPublisher,
        CreateOrderRequest request)
    {
        var order = new Order
        {
            CustomerName = request.CustomerName,
            Description = request.Description,
            UnitPrice = request.UnitPrice,
            Quantity = request.Quantity,
            Status = OrderStatus.Created,
            CreatedAt = DateTime.UtcNow
        };

        dbContext.Orders.Add(order);
        await dbContext.SaveChangesAsync();

        // Publish domain event
        await eventPublisher.PublishAsync(new OrderCreatedEvent(
            order.Id,
            order.CustomerName,
            order.Description,
            order.TotalAmount,
            order.Status,
            order.CreatedAt));

        return TypedResults.Created($"/orders/{order.Id}", order.Id);
    }
} 