using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Invoices.Infrastructure;
using Invoices.Features.Shared;

namespace Invoices.Features.GetInvoice;

public static class GetInvoiceEndpoint
{
    public static IEndpointRouteBuilder MapGetInvoice(this IEndpointRouteBuilder app)
    {
        app.MapGet("/invoices/{id}", GetInvoiceById)
           .WithName("GetInvoice")
           .WithOpenApi();

        return app;
    }

    private static async Task<Results<Ok<GetInvoiceResponse>, NotFound>> GetInvoiceById(InvoiceDbContext dbContext, int id)
    {
        var invoice = await dbContext.Invoices
            .Include(i => i.Items)
            .Include(i => i.Payments)
            .Where(i => i.Id == id)
            .Select(i => new GetInvoiceResponse(
                i.Id,
                i.Number,
                i.CustomerName,
                i.TotalAmount,
                i.PaidAmount,
                i.RemainingAmount,
                i.IssueDate,
                i.DueDate,
                i.PaymentReference,
                i.OrderId,
                i.Status,
                i.CreatedAt,
                i.PaidAt,
                i.Items.Select(item => new InvoiceItemResponse(
                    item.Id,
                    item.Description,
                    item.UnitPrice,
                    item.Quantity,
                    item.TotalAmount)).ToList(),
                i.Payments.Select(p => new InvoicePaymentResponse(
                    p.Id,
                    p.Amount,
                    p.PaymentReference,
                    p.PaidAt)).ToList()))
            .FirstOrDefaultAsync();

        if (invoice is null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(invoice);
    }
} 