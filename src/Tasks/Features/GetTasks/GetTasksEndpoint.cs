using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Tasks.Infrastructure;
using Tasks.Features.GetTask;

namespace Tasks.Features.GetTasks;

public static class GetTasksEndpoint
{
    public static IEndpointRouteBuilder MapGetTasks(this IEndpointRouteBuilder app)
    {
        app.MapGet("/tasks", GetAllTasks)
           .WithName("GetTasks")
           .WithOpenApi();

        return app;
    }

    private static async Task<Ok<List<GetTaskResponse>>> GetAllTasks(TaskDbContext dbContext)
    {
        var tasks = await dbContext.Tasks
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
            .ToListAsync();

        return TypedResults.Ok(tasks);
    }
} 