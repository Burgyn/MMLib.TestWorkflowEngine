using Aspire.Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults (telemetry, health checks, etc.)
builder.AddServiceDefaults();

// Add Azure Service Bus
builder.AddAzureServiceBusClient("messaging");

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddHostedService<MessageConsumerService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapDefaultEndpoints(); // Health checks, etc.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.Run();

// Message Consumer Service
public class MessageConsumerService : BackgroundService
{
    private readonly ServiceBusClient _client;
    private readonly ILogger<MessageConsumerService> _logger;
    private ServiceBusProcessor? _processor;

    public MessageConsumerService(ServiceBusClient client, ILogger<MessageConsumerService> logger)
    {
        _client = client;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _processor = _client.CreateProcessor(
            topicName: "domain-events",
            subscriptionName: "tasks-subscription",
            new ServiceBusProcessorOptions
            {
                MaxConcurrentCalls = 1,
                AutoCompleteMessages = true
            });

        _processor.ProcessMessageAsync += MessageHandler;
        _processor.ProcessErrorAsync += ErrorHandler;

        await _processor.StartProcessingAsync(stoppingToken);
    }

    private async Task MessageHandler(ProcessMessageEventArgs args)
    {
        var body = args.Message.Body.ToString();
        _logger.LogInformation(
            "Received message: Subject = {Subject}, Body = {Body}",
            args.Message.Subject,
            body);
    }

    private Task ErrorHandler(ProcessErrorEventArgs args)
    {
        _logger.LogError(
            args.Exception,
            "Error processing message: {ErrorSource}, {FullyQualifiedNamespace}",
            args.ErrorSource,
            args.FullyQualifiedNamespace);

        return Task.CompletedTask;
    }

    public override async Task StopAsync(CancellationToken stoppingToken)
    {
        if (_processor != null)
        {
            await _processor.StopProcessingAsync(stoppingToken);
            await _processor.DisposeAsync();
        }
    }
}

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
