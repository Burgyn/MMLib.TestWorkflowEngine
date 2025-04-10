using Orders.Infrastructure;

namespace Orders.Features.UpdateOrderStatus;

public static class UpdateOrderStatusEndpoint
{
    public static IEndpointRouteBuilder MapUpdateOrderStatus(this IEndpointRouteBuilder app)
    {
        app.MapPut("/orders/{id}/status", async (OrderDbContext dbContext, int id, UpdateOrderStatusRequest request) =>
        {
            var order = await dbContext.Orders.FindAsync(id);
            
            if (order == null)
            {
                return Results.NotFound();
            }

            order.Status = request.NewStatus;
            order.LastModifiedAt = DateTime.UtcNow;
            
            await dbContext.SaveChangesAsync();
            return Results.NoContent();
        })
        .WithName("UpdateOrderStatus")
        .WithOpenApi();

        return app;
    }
} 