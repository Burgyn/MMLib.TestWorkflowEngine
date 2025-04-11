using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Invoices.Domain;
using Invoices.Domain.Events;
using Invoices.Infrastructure;
using Invoices.Infrastructure.DomainEvents;

namespace Invoices.Features.CreateInvoice;

public static class CreateInvoiceEndpoint
{
    public static IEndpointRouteBuilder MapCreateInvoice(this IEndpointRouteBuilder app)
    {
        app.MapPost("/invoices", CreateInvoice)
           .WithName("CreateInvoice")
           .WithOpenApi();

        return app;
    }

    private static async Task<Created<int>> CreateInvoice(
        InvoiceDbContext dbContext,
        IEventPublisher eventPublisher,
        CreateInvoiceRequest request)
    {
        var lastInvoice = await dbContext.Invoices
            .OrderByDescending(i => i.Number)
            .FirstOrDefaultAsync();

        var nextNumber = lastInvoice != null 
            ? (int.Parse(lastInvoice.Number) + 1).ToString("D7")
            : "2024001";

        var invoice = new Invoice
        {
            Number = nextNumber,
            CustomerName = request.CustomerName,
            IssueDate = DateTime.UtcNow,
            DueDate = request.DueDate,
            Status = InvoiceStatus.Created,
            CreatedAt = DateTime.UtcNow,
            OrderId = request.OrderId,
            Items = request.Items.Select(item => new InvoiceItem
            {
                Description = item.Description,
                UnitPrice = item.UnitPrice,
                Quantity = item.Quantity
            }).ToList()
        };

        dbContext.Invoices.Add(invoice);
        await dbContext.SaveChangesAsync();

        // Publish domain event
        await eventPublisher.PublishAsync(new InvoiceCreatedEvent(
            invoice.Id,
            invoice.Number,
            invoice.CustomerName,
            invoice.TotalAmount,
            invoice.IssueDate,
            invoice.DueDate,
            invoice.Status,
            invoice.CreatedAt,
            invoice.OrderId));

        return TypedResults.Created($"/invoices/{invoice.Id}", invoice.Id);
    }
} 