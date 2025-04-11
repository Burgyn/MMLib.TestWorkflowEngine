using Azure.Messaging.ServiceBus;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults
builder.AddServiceDefaults();

// Add Azure Service Bus
builder.AddAzureServiceBusClient("messaging");

// Add n8n service reference with service discovery
builder.Services.AddHttpClient("n8n", client =>
{
    var n8nUri = "http://localhost:5678";
    client.BaseAddress = new Uri(n8nUri);
});

// Add HttpClient
builder.Services.AddHttpClient();

// Add services to the container
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline
app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Get services
var serviceBusClient = app.Services.GetRequiredService<ServiceBusClient>();
var httpClientFactory = app.Services.GetRequiredService<IHttpClientFactory>();
var logger = app.Logger;
var n8nHttpClient = httpClientFactory.CreateClient("n8n");
var n8nWebhookPath = "/webhook-test/43a23d54-fcdf-497d-9b1f-0dace15cf79e";

// Create processor for each subscription
await CreateProcessor("orders-subscription", "Orders");
await CreateProcessor("invoices-subscription", "Invoices");
await CreateProcessor("tasks-subscription", "Tasks");

async Task CreateProcessor(string subscriptionName, string source)
{
    var processor = serviceBusClient.CreateProcessor("domain-events", subscriptionName);

    processor.ProcessMessageAsync += async args =>
    {
        try
        {
            var body = args.Message.Body.ToString();
            logger.LogInformation("Received message from {Source}: {Body}", source, body);

            // Create payload for n8n
            var payload = new
            {
                source = source,
                messageId = args.Message.MessageId,
                correlationId = args.Message.CorrelationId,
                data = JsonSerializer.Deserialize<JsonElement>(body),
                metadata = new
                {
                    enqueuedTime = args.Message.EnqueuedTime,
                    sequenceNumber = args.Message.SequenceNumber,
                    subject = args.Message.Subject,
                    contentType = args.Message.ContentType
                }
            };

            // Forward to n8n using the named client
            var response = await n8nHttpClient.PostAsJsonAsync(n8nWebhookPath, payload);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to forward message to n8n. Status code: {response.StatusCode}");
            }

            logger.LogInformation("Successfully forwarded message to n8n");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error processing message from {Source}", source);
            throw;
        }
    };

    processor.ProcessErrorAsync += args =>
    {
        logger.LogError(args.Exception, "Error in processor for {Source}", source);
        return Task.CompletedTask;
    };

    await processor.StartProcessingAsync();
}

app.Run();