using Microsoft.EntityFrameworkCore;
using Orders.Infrastructure;

namespace Orders.Features.GetOrder;

public static class GetOrderEndpoint
{
    public static IEndpointRouteBuilder MapGetOrder(this IEndpointRouteBuilder app)
    {
        app.MapGet("/orders/{id}", async (OrderDbContext dbContext, int id) =>
        {
            var order = await dbContext.Orders
                .Where(o => o.Id == id)
                .Select(o => new GetOrderResponse(
                    o.Id,
                    o.CustomerName,
                    o.Description,
                    o.TotalAmount,
                    o.Status,
                    o.CreatedAt,
                    o.LastModifiedAt))
                .FirstOrDefaultAsync();

            return order is not null ? Results.Ok(order) : Results.NotFound();
        })
        .WithName("GetOrder")
        .WithOpenApi();

        return app;
    }
} 