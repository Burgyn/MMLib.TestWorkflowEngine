using Microsoft.EntityFrameworkCore;
using Invoices.Features.GetInvoice;
using Invoices.Features.GetInvoices;
using Invoices.Features.CreateInvoice;
using Invoices.Features.PayInvoice;
using Invoices.Infrastructure;

namespace Invoices.Features;

public static class InvoiceFeatures
{
    public static IServiceCollection AddInvoices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<InvoiceDbContext>(options =>
            options.UseInMemoryDatabase("InvoicesDb"));

        return services;
    }

    public static IEndpointRouteBuilder MapInvoices(this IEndpointRouteBuilder app)
    {
        app.MapGetInvoice()
           .MapGetInvoices()
           .MapCreateInvoice()
           .MapPayInvoice();

        return app;
    }

    public static WebApplication UseInvoices(this WebApplication app)
    {
        // Initialize database
        using (var scope = app.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<InvoiceDbContext>();
            context.Database.EnsureCreated();
        }

        return app;
    }
} 