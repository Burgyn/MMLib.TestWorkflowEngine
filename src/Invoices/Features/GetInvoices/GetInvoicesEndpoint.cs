using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Invoices.Infrastructure;
using Invoices.Features.Shared;

namespace Invoices.Features.GetInvoices;

public static class GetInvoicesEndpoint
{
    public static IEndpointRouteBuilder MapGetInvoices(this IEndpointRouteBuilder app)
    {
        app.MapGet("/invoices", GetAllInvoices)
           .WithName("GetInvoices")
           .WithOpenApi();

        return app;
    }

    private static async Task<Ok<List<GetInvoicesResponse>>> GetAllInvoices(InvoiceDbContext dbContext)
    {
        var invoices = await dbContext.Invoices
            .Include(i => i.Items)
            .Include(i => i.Payments)
            .Select(i => new GetInvoicesResponse(
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
            .ToListAsync();

        return TypedResults.Ok(invoices);
    }
} 