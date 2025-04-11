using Microsoft.AspNetCore.Http.HttpResults;
using Tasks.Domain;
using Tasks.Domain.Events;
using Tasks.Infrastructure;
using Tasks.Infrastructure.DomainEvents;

namespace Tasks.Features.CompleteTask;

public static class CompleteTaskEndpoint
{
    public static IEndpointRouteBuilder MapCompleteTask(this IEndpointRouteBuilder app)
    {
        app.MapPost("/tasks/{id}/complete", CompleteTask)
           .WithName("CompleteTask")
           .WithOpenApi();

        return app;
    }

    private static async Task<Results<NoContent, NotFound, BadRequest<string>>> CompleteTask(
        TaskDbContext dbContext,
        IEventPublisher eventPublisher,
        int id,
        CompleteTaskRequest request)
    {
        var task = await dbContext.Tasks.FindAsync(id);

        if (task == null)
        {
            return TypedResults.NotFound();
        }

        if (task.State == State.Completed)
        {
            return TypedResults.BadRequest("Task is already completed.");
        }

        task.State = State.Completed;
        task.CompletedAt = request.CompletedAt;

        await dbContext.SaveChangesAsync();

        // Publish domain event
        await eventPublisher.PublishAsync(new TaskCompletedEvent(
            task.Id,
            task.AssigneeEmail,
            task.Description,
            task.StartDate,
            task.DueDate,
            task.State,
            task.OrderId,
            task.CreatedAt,
            task.CompletedAt!.Value));

        return TypedResults.NoContent();
    }
}