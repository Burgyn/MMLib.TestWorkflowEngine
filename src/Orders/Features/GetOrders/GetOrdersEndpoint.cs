using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Orders.Infrastructure;

namespace Orders.Features.GetOrders;

public static class GetOrdersEndpoint
{
    public static IEndpointRouteBuilder MapGetOrders(this IEndpointRouteBuilder app)
    {
        app.MapGet("/orders", GetAllOrders)
           .WithName("GetOrders")
           .WithOpenApi();

        return app;
    }

    private static async Task<Ok<List<GetOrdersResponse>>> GetAllOrders(OrderDbContext dbContext)
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

        return TypedResults.Ok(orders);
    }
} 