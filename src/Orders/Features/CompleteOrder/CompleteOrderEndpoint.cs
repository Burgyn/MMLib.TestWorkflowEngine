using Microsoft.AspNetCore.Http.HttpResults;
using Orders.Domain;
using Orders.Domain.Events;
using Orders.Infrastructure;
using Orders.Infrastructure.DomainEvents;

namespace Orders.Features.CompleteOrder;

public static class CompleteOrderEndpoint
{
    public static IEndpointRouteBuilder MapCompleteOrder(this IEndpointRouteBuilder app)
    {
        app.MapPost("/orders/{id}/complete", CompleteOrder)
           .WithName("CompleteOrder")
           .WithOpenApi();

        return app;
    }

    private static async Task<Results<NoContent, NotFound, BadRequest<string>>> CompleteOrder(
        OrderDbContext dbContext,
        IEventPublisher eventPublisher,
        int id,
        CompleteOrderRequest request)
    {
        var order = await dbContext.Orders.FindAsync(id);

        if (order == null)
        {
            return TypedResults.NotFound();
        }

        if (order.Status == OrderStatus.Completed)
        {
            return TypedResults.BadRequest("Order is already completed.");
        }

        if (order.Status == OrderStatus.Invoiced)
        {
            return TypedResults.BadRequest("Cannot complete order that is already invoiced.");
        }

        order.Status = OrderStatus.Completed;
        order.LastModifiedAt = request.CompletedAt;
            
        await dbContext.SaveChangesAsync();

        // Publish domain event
        await eventPublisher.PublishAsync(new OrderCompletedEvent(
            order.Id,
            order.CustomerName,
            order.Description,
            order.TotalAmount,
            order.Status,
            order.CreatedAt,
            order.LastModifiedAt!.Value));

        return TypedResults.NoContent();
    }
} 