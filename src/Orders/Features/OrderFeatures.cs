using Microsoft.EntityFrameworkCore;
using Orders.Features.GetOrder;
using Orders.Features.GetOrders;
using Orders.Features.CreateOrder;
using Orders.Features.UpdateOrderStatus;
using Orders.Infrastructure;

namespace Orders.Features;

public static class OrderFeatures
{
    public static IServiceCollection AddOrders(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<OrderDbContext>(options =>
            options.UseInMemoryDatabase("OrdersDb"));

        return services;
    }

    public static IEndpointRouteBuilder MapOrders(this IEndpointRouteBuilder app)
    {
        app.MapGetOrder()
           .MapGetOrders()
           .MapCreateOrder()
           .MapUpdateOrderStatus();

        return app;
    }

    public static WebApplication UseOrders(this WebApplication app)
    {
        // Initialize database
        using (var scope = app.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<OrderDbContext>();
            context.Database.EnsureCreated();
        }

        return app;
    }
} 