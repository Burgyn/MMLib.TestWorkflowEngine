using Microsoft.AspNetCore.Http.HttpResults;
using Tasks.Domain;
using Tasks.Domain.Events;
using Tasks.Infrastructure;
using Tasks.Infrastructure.DomainEvents;

namespace Tasks.Features.CreateTask;

public static class CreateTaskEndpoint
{
    public static IEndpointRouteBuilder MapCreateTask(this IEndpointRouteBuilder app)
    {
        app.MapPost("/tasks", CreateTask)
           .WithName("CreateTask")
           .WithOpenApi();

        return app;
    }

    private static async Task<Created<int>> CreateTask(
        TaskDbContext dbContext,
        IEventPublisher eventPublisher,
        CreateTaskRequest request)
    {
        var task = new Domain.Task
        {
            AssigneeEmail = request.AssigneeEmail,
            Description = request.Description,
            StartDate = request.StartDate,
            DueDate = request.DueDate,
            OrderId = request.OrderId,
            State = State.Created,
            CreatedAt = DateTime.UtcNow
        };

        dbContext.Tasks.Add(task);
        await dbContext.SaveChangesAsync();

        // Publish domain event
        await eventPublisher.PublishAsync(new TaskCreatedEvent(
            task.Id,
            task.AssigneeEmail,
            task.Description,
            task.StartDate,
            task.DueDate,
            task.State,
            task.OrderId,
            task.CreatedAt));

        return TypedResults.Created($"/tasks/{task.Id}", task.Id);
    }
} 