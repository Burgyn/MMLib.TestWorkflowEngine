using Microsoft.AspNetCore.Http.HttpResults;
using Orders.Domain;
using Orders.Domain.Events;
using Orders.Infrastructure;
using Orders.Infrastructure.DomainEvents;

namespace Orders.Features.UpdateOrderStatus;

public static class UpdateOrderStatusEndpoint
{
    public static IEndpointRouteBuilder MapUpdateOrderStatus(this IEndpointRouteBuilder app)
    {
        app.MapPut("/orders/{id}/status", UpdateStatus)
           .WithName("UpdateOrderStatus")
           .WithOpenApi();

        return app;
    }

    private static async Task<Results<NoContent, NotFound>> UpdateStatus(
        OrderDbContext dbContext,
        IEventPublisher eventPublisher,
        int id,
        UpdateOrderStatusRequest request)
    {
        var order = await dbContext.Orders.FindAsync(id);

        if (order == null)
        {
            return TypedResults.NotFound();
        }

        order.Status = request.NewStatus;
        order.LastModifiedAt = DateTime.UtcNow;

        await dbContext.SaveChangesAsync();

        // Publish domain event
        await eventPublisher.PublishAsync(new OrderStatusChangedEvent(
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