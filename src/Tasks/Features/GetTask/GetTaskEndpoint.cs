using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Tasks.Infrastructure;

namespace Tasks.Features.GetTask;

public static class GetTaskEndpoint
{
    public static IEndpointRouteBuilder MapGetTask(this IEndpointRouteBuilder app)
    {
        app.MapGet("/tasks/{id}", GetTaskById)
           .WithName("GetTask")
           .WithOpenApi();

        return app;
    }

    private static async Task<Results<Ok<GetTaskResponse>, NotFound>> GetTaskById(TaskDbContext dbContext, int id)
    {
        var task = await dbContext.Tasks
            .Where(t => t.Id == id)
            .Select(t => new GetTaskResponse(
                t.Id,
                t.AssigneeEmail,
                t.Description,
                t.StartDate,
                t.DueDate,
                t.State,
                t.OrderId,
                t.CreatedAt,
                t.LastModifiedAt))
            .FirstOrDefaultAsync();

        if (task is null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(task);
    }
} 