using Elsa.Studio.Dashboard.Extensions;
using Elsa.Studio.Shell;
using Elsa.Studio.Shell.Extensions;
using Elsa.Studio.Workflows.Extensions;
using Elsa.Studio.Contracts;
using Elsa.Studio.Core.BlazorWasm.Extensions;
using Elsa.Studio.Extensions;
using Elsa.Studio.Login.BlazorWasm.Extensions;
using Elsa.Studio.Login.HttpMessageHandlers;
using Elsa.Studio.Workflows.Designer.Extensions;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Elsa.Studio.Models;
using Microsoft.Extensions.Logging;

// Build the host.
var builder = WebAssemblyHostBuilder.CreateDefault(args);
var configuration = builder.Configuration;

builder.Services.AddLogging(logging => logging
    .SetMinimumLevel(LogLevel.Debug)
    .AddFilter("Microsoft", LogLevel.Warning)
    .AddFilter("System", LogLevel.Warning));
// Register root components.
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.RootComponents.RegisterCustomElsaStudioElements();

// Get logger
var logger = builder.Services.BuildServiceProvider().GetRequiredService<ILoggerFactory>()
    .CreateLogger<Program>();

// Register shell services and modules.
var backendApiConfig = new BackendApiConfig
{
    ConfigureBackendOptions = options =>
    {
        var backendSection = builder.Configuration.GetSection("Backend");
        backendSection.Bind(options);

        // Override URL from environment if provided
        var backendUrl = builder.Configuration["Backend__Url"];
        logger.LogInformation("Backend URL from configuration: {BackendUrl}", backendUrl);
        logger.LogInformation("Current Backend URL from options: {CurrentUrl}", options.Url);

        if (!string.IsNullOrEmpty(backendUrl))
        {
            options.Url = new Uri(backendUrl);
            logger.LogInformation("Updated Backend URL to: {UpdatedUrl}", options.Url);
        }
    },
    ConfigureHttpClientBuilder = options => options.AuthenticationHandler = typeof(AuthenticatingApiHttpMessageHandler)
};

builder.Services.AddCore();
builder.Services.AddShell();
builder.Services.AddRemoteBackend(backendApiConfig);
builder.Services.AddLoginModule();
builder.Services.AddDashboardModule();
builder.Services.AddWorkflowsModule();

// Build the application.
var app = builder.Build();

// Run each startup task.
var startupTaskRunner = app.Services.GetRequiredService<IStartupTaskRunner>();
await startupTaskRunner.RunStartupTasksAsync();

// Run the application.
await app.RunAsync();