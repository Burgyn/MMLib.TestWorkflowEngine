using Microsoft.EntityFrameworkCore;
using Tasks.Features.GetTask;
using Tasks.Features.GetTasks;
using Tasks.Features.CreateTask;
using Tasks.Infrastructure;
using Tasks.Infrastructure.DomainEvents;

namespace Tasks.Features;

public static class TaskFeatures
{
    public static IServiceCollection AddTasks(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<TaskDbContext>(options =>
            options.UseInMemoryDatabase("TasksDb"));

        services.AddScoped<IEventPublisher, ServiceBusEventPublisher>();

        return services;
    }

    public static IEndpointRouteBuilder MapTasks(this IEndpointRouteBuilder app)
    {
        app.MapGetTask()
           .MapGetTasks()
           .MapCreateTask();

        return app;
    }

    public static WebApplication UseTasks(this WebApplication app)
    {
        // Initialize database
        using (var scope = app.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<TaskDbContext>();
            context.Database.EnsureCreated();
        }

        return app;
    }
} 