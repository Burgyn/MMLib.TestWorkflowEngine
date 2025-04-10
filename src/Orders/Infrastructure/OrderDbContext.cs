using Microsoft.EntityFrameworkCore;
using Orders.Domain;

namespace Orders.Infrastructure;

public class OrderDbContext : DbContext
{
    public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options)
    {
    }

    public DbSet<Order> Orders => Set<Order>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Seed data
        modelBuilder.Entity<Order>().HasData(
            new Order
            {
                Id = 1,
                CustomerName = "Peter Novák",
                Description = "iPhone 15 Pro 256GB Space Black",
                UnitPrice = 1199.99m,
                Quantity = 1,
                Status = OrderStatus.Created,
                CreatedAt = DateTime.UtcNow
            },
            new Order
            {
                Id = 2,
                CustomerName = "Jana Kováčová",
                Description = "MacBook Air M2 16GB/512GB",
                UnitPrice = 1399.50m,
                Quantity = 1,
                Status = OrderStatus.InProgress,
                CreatedAt = DateTime.UtcNow
            },
            new Order
            {
                Id = 3,
                CustomerName = "Martin Horváth",
                Description = "Samsung 32\" Gaming Monitor Odyssey G7",
                UnitPrice = 699.99m,
                Quantity = 2,
                Status = OrderStatus.Waiting,
                CreatedAt = DateTime.UtcNow,
                LastModifiedAt = DateTime.UtcNow
            },
            new Order
            {
                Id = 4,
                CustomerName = "Eva Tóthová",
                Description = "Sony WH-1000XM5 Wireless Headphones",
                UnitPrice = 379.99m,
                Quantity = 1,
                Status = OrderStatus.Completed,
                CreatedAt = DateTime.UtcNow.AddDays(-2),
                LastModifiedAt = DateTime.UtcNow.AddDays(-1)
            },
            new Order
            {
                Id = 5,
                CustomerName = "Tomáš Balog",
                Description = "ASUS ROG Strix Gaming Laptop",
                UnitPrice = 1899.99m,
                Quantity = 1,
                Status = OrderStatus.Invoiced,
                CreatedAt = DateTime.UtcNow.AddDays(-5),
                LastModifiedAt = DateTime.UtcNow.AddDays(-3)
            }
        );
    }
} 