using Microsoft.EntityFrameworkCore;
using Invoices.Domain;

namespace Invoices.Infrastructure;

public class InvoiceDbContext : DbContext
{
    public InvoiceDbContext(DbContextOptions<InvoiceDbContext> options) : base(options)
    {
    }

    public DbSet<Invoice> Invoices => Set<Invoice>();
    public DbSet<InvoiceItem> InvoiceItems => Set<InvoiceItem>();
    public DbSet<InvoicePayment> InvoicePayments => Set<InvoicePayment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Invoice>()
            .HasMany(i => i.Items)
            .WithOne(i => i.Invoice)
            .HasForeignKey(i => i.InvoiceId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Invoice>()
            .HasMany(i => i.Payments)
            .WithOne(p => p.Invoice)
            .HasForeignKey(p => p.InvoiceId)
            .OnDelete(DeleteBehavior.Cascade);

        // Seed data
        var invoice1 = new Invoice
        {
            Id = 1,
            Number = "2024001",
            CustomerName = "Peter Novák",
            IssueDate = DateTime.UtcNow.AddDays(-15),
            DueDate = DateTime.UtcNow.AddDays(15),
            Status = InvoiceStatus.Unpaid,
            CreatedAt = DateTime.UtcNow.AddDays(-15),
            OrderId = 1
        };

        var invoice2 = new Invoice
        {
            Id = 2,
            Number = "2024002",
            CustomerName = "Jana Kováčová",
            IssueDate = DateTime.UtcNow.AddDays(-10),
            DueDate = DateTime.UtcNow.AddDays(20),
            Status = InvoiceStatus.Created,
            CreatedAt = DateTime.UtcNow.AddDays(-10),
            OrderId = 2
        };

        var invoice3 = new Invoice
        {
            Id = 3,
            Number = "2024003",
            CustomerName = "Martin Horváth",
            IssueDate = DateTime.UtcNow.AddDays(-30),
            DueDate = DateTime.UtcNow,
            Status = InvoiceStatus.Paid,
            CreatedAt = DateTime.UtcNow.AddDays(-30),
            PaidAt = DateTime.UtcNow.AddDays(-5),
            PaidAmount = 1399.98m
        };

        modelBuilder.Entity<Invoice>().HasData(invoice1, invoice2, invoice3);

        modelBuilder.Entity<InvoicePayment>().HasData(
            new InvoicePayment
            {
                Id = 1,
                InvoiceId = 3,
                Amount = 1399.98m,
                PaymentReference = "PO-2024-123",
                PaidAt = DateTime.UtcNow.AddDays(-5)
            }
        );

        modelBuilder.Entity<InvoiceItem>().HasData(
            new InvoiceItem 
            { 
                Id = 1,
                InvoiceId = 1,
                Description = "iPhone 15 Pro 256GB",
                UnitPrice = 1199.99m,
                Quantity = 1
            },
            new InvoiceItem 
            { 
                Id = 2,
                InvoiceId = 1,
                Description = "AppleCare+ for iPhone 15 Pro",
                UnitPrice = 199.99m,
                Quantity = 1
            },
            new InvoiceItem 
            { 
                Id = 3,
                InvoiceId = 2,
                Description = "MacBook Air M2 16GB/512GB",
                UnitPrice = 1399.50m,
                Quantity = 1
            },
            new InvoiceItem 
            { 
                Id = 4,
                InvoiceId = 2,
                Description = "Magic Mouse",
                UnitPrice = 99.99m,
                Quantity = 1
            },
            new InvoiceItem 
            { 
                Id = 5,
                InvoiceId = 3,
                Description = "Samsung 32\" Gaming Monitor Odyssey G7",
                UnitPrice = 699.99m,
                Quantity = 2
            }
        );
    }
} 