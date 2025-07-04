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

        if (request.Amount <= 0)
        {
            return TypedResults.BadRequest("Payment amount must be greater than zero.");
        }

        invoice.AddPayment(request.Amount, request.PaymentReference);
        await dbContext.SaveChangesAsync();

        // Publish domain event
        await eventPublisher.PublishAsync(new InvoicePaidEvent(
            invoice.Id,
            invoice.Number,
            invoice.CustomerName,
            invoice.TotalAmount,
            invoice.PaidAmount,
            request.Amount,
            request.PaymentReference,
            DateTime.UtcNow));

        return TypedResults.NoContent();
    }
} 