using Microsoft.AspNetCore.Http.HttpResults;
using Invoices.Domain;
using Invoices.Domain.Events;
using Invoices.Infrastructure;
using Invoices.Infrastructure.DomainEvents;

namespace Invoices.Features.PayInvoice;

public static class PayInvoiceEndpoint
{
    public static IEndpointRouteBuilder MapPayInvoice(this IEndpointRouteBuilder app)
    {
        app.MapPut("/invoices/{id}/pay", PayInvoice)
           .WithName("PayInvoice")
           .WithOpenApi();

        return app;
    }

    private static async Task<Results<NoContent, NotFound, BadRequest<string>>> PayInvoice(
        InvoiceDbContext dbContext,
        IEventPublisher eventPublisher,
        int id,
        PayInvoiceRequest request)
    {
        var invoice = await dbContext.Invoices.FindAsync(id);
            
        if (invoice == null)
        {
            return TypedResults.NotFound();
        }

        if (invoice.Status == InvoiceStatus.Paid)
        {
            return TypedResults.BadRequest("Invoice is already paid.");
        }

        invoice.Status = InvoiceStatus.Paid;
        invoice.PaymentReference = request.PaymentReference;
        invoice.PaidAt = DateTime.UtcNow;
            
        await dbContext.SaveChangesAsync();

        // Publish domain event
        await eventPublisher.PublishAsync(new InvoicePaidEvent(
            invoice.Id,
            invoice.Number,
            invoice.CustomerName,
            invoice.TotalAmount,
            invoice.PaymentReference!,
            invoice.PaidAt!.Value));

        return TypedResults.NoContent();
    }
} 