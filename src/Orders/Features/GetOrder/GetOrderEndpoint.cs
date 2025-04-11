using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Orders.Infrastructure;

namespace Orders.Features.GetOrder;

public static class GetOrderEndpoint
{
    public static IEndpointRouteBuilder MapGetOrder(this IEndpointRouteBuilder app)
    {
        app.MapGet("/orders/{id}", GetOrderById)
           .WithName("GetOrder")
           .WithOpenApi();

        return app;
    }

    private static async Task<Results<Ok<GetOrderResponse>, NotFound>> GetOrderById(OrderDbContext dbContext, int id)
    {
        var order = await dbContext.Orders
            .Where(o => o.Id == id)
            .Select(o => new GetOrderResponse(
                o.Id,
                o.CustomerName,
                o.Description,
                o.UnitPrice,
                o.Quantity,
                o.TotalAmount,
                o.Status,
                o.CreatedAt,
                o.LastModifiedAt))
            .FirstOrDefaultAsync();

        if (order is null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(order);
    }
} 