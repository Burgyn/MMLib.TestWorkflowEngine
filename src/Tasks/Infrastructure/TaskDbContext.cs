using Microsoft.EntityFrameworkCore;
using Tasks.Domain;

namespace Tasks.Infrastructure;

public class TaskDbContext : DbContext
{
    public TaskDbContext(DbContextOptions<TaskDbContext> options) : base(options)
    {
    }

    public DbSet<Domain.Task> Tasks => Set<Domain.Task>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Seed data
        modelBuilder.Entity<Domain.Task>().HasData(
            new Domain.Task
            {
                Id = 1,
                AssigneeEmail = "peter.novak@example.com",
                Description = "Prepare shipping documents for iPhone 15 Pro order",
                StartDate = DateTime.UtcNow,
                DueDate = DateTime.UtcNow.AddDays(2),
                State = State.Created,
                OrderId = 1,
                CreatedAt = DateTime.UtcNow
            },
            new Domain.Task
            {
                Id = 2,
                AssigneeEmail = "jana.kovacova@example.com",
                Description = "Quality check for MacBook Air M2",
                StartDate = DateTime.UtcNow.AddDays(-1),
                DueDate = DateTime.UtcNow.AddDays(1),
                State = State.InProgress,
                OrderId = 2,
                CreatedAt = DateTime.UtcNow.AddDays(-1),
                LastModifiedAt = DateTime.UtcNow
            },
            new Domain.Task
            {
                Id = 3,
                AssigneeEmail = "martin.horvath@example.com",
                Description = "Monthly team performance review",
                StartDate = DateTime.UtcNow,
                DueDate = DateTime.UtcNow.AddDays(5),
                State = State.Created,
                CreatedAt = DateTime.UtcNow
            }
        );
    }
} 