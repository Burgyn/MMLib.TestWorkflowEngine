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
var baseWebhookPath = "/webhook/43a23d54-fcdf-497d-9b1f-0dace15cf79e";
//var baseWebhookPath = "/webhook-test/43a23d54-fcdf-497d-9b1f-0dace15cf79e";

// Create processor for each event type
logger.LogInformation("Setting up processors for Service Bus topics...");

await CreateProcessor("order-created", "order-created-sub", "OrderCreated");
await CreateProcessor("order-status-changed", "order-status-changed-sub", "OrderStatusChanged");
await CreateProcessor("invoice-created", "invoice-created-sub", "InvoiceCreated");
await CreateProcessor("invoice-paid", "invoice-paid-sub", "InvoicePaid");
await CreateProcessor("task-created", "task-created-sub", "TaskCreated");
await CreateProcessor("task-completed", "task-completed-sub", "TaskCompleted");

logger.LogInformation("All processors are set up and running");

async Task CreateProcessor(string topicName, string subscriptionName, string eventType)
{
    logger.LogInformation("Creating processor for topic: {TopicName}, subscription: {SubscriptionName}",
        topicName, subscriptionName);

    var processor = serviceBusClient.CreateProcessor(topicName, subscriptionName);

    processor.ProcessMessageAsync += async args =>
    {
        try
        {
            var body = args.Message.Body.ToString();
            logger.LogInformation(
                "Received message:\nType: {EventType}\nTopic: {TopicName}\nSubscription: {SubscriptionName}\nBody: {Body}",
                eventType, topicName, subscriptionName, body);

            // Create payload for n8n
            var payload = new
            {
                type = eventType,
                data = JsonSerializer.Deserialize<JsonElement>(body)
            };

            // Send to n8n webhook
            var webhookPath = $"{baseWebhookPath}/{topicName}";
            logger.LogInformation("Sending to n8n webhook: {WebhookPath}", webhookPath);

            var response = await n8nHttpClient.PostAsJsonAsync(webhookPath, payload);
            var responseContent = await response.Content.ReadAsStringAsync();

            logger.LogInformation(
                "n8n response:\nStatus: {Status}\nContent: {Content}",
                response.StatusCode, responseContent);

            response.EnsureSuccessStatusCode();
            logger.LogInformation("Successfully forwarded {EventType} event to n8n", eventType);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error processing {EventType} event from topic {TopicName}", eventType, topicName);
        }
    };

    processor.ProcessErrorAsync += args =>
    {
        logger.LogError(args.Exception, "Error in processor for topic {TopicName}", topicName);
        return Task.CompletedTask;
    };

    await processor.StartProcessingAsync();
    logger.LogInformation("Started processor for topic: {TopicName}", topicName);
}

app.Run();