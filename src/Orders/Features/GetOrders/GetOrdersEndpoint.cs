using Microsoft.EntityFrameworkCore;
using Orders.Infrastructure;

namespace Orders.Features.GetOrders;

public static class GetOrdersEndpoint
{
    public static IEndpointRouteBuilder MapGetOrders(this IEndpointRouteBuilder app)
    {
        app.MapGet("/orders", async (OrderDbContext dbContext) =>
        {
            var orders = await dbContext.Orders
                .Select(o => new GetOrdersResponse(
                    o.Id,
                    o.CustomerName,
                    o.Description,
                    o.TotalAmount,
                    o.Status,
                    o.CreatedAt,
                    o.LastModifiedAt))
                .ToListAsync();

            return Results.Ok(orders);
        })
        .WithName("GetOrders")
        .WithOpenApi();

        return app;
    }
} 