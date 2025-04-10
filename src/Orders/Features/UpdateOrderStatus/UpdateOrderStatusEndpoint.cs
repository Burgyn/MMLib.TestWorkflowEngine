using Microsoft.AspNetCore.Http.HttpResults;
using Orders.Infrastructure;

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
        return TypedResults.NoContent();
    }
}