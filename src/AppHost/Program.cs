using Aspire.Hosting;
using System.IO;

var builder = DistributedApplication.CreateBuilder(args);

// Set Aspire store path
builder.Configuration["Aspire:Store:Path"] = Path.Combine(Directory.GetCurrentDirectory(), "aspire-store");

// Add Azure Service Bus with emulator
var serviceBus = builder.AddAzureServiceBus("messaging");

// Configure topics for each specific event
// Order events
var orderCreatedTopic = serviceBus.AddServiceBusTopic("order-created");
orderCreatedTopic.AddServiceBusSubscription("order-created-sub");

var orderStatusChangedTopic = serviceBus.AddServiceBusTopic("order-status-changed");
orderStatusChangedTopic.AddServiceBusSubscription("order-status-changed-sub");

var orderCompletedTopic = serviceBus.AddServiceBusTopic("order-completed");
orderCompletedTopic.AddServiceBusSubscription("order-completed-sub");

// Invoice events
var invoiceCreatedTopic = serviceBus.AddServiceBusTopic("invoice-created");
invoiceCreatedTopic.AddServiceBusSubscription("invoice-created-sub");

var invoicePaidTopic = serviceBus.AddServiceBusTopic("invoice-paid");
invoicePaidTopic.AddServiceBusSubscription("invoice-paid-sub");

// Task events
var taskCreatedTopic = serviceBus.AddServiceBusTopic("task-created");
taskCreatedTopic.AddServiceBusSubscription("task-created-sub");

var taskCompletedTopic = serviceBus.AddServiceBusTopic("task-completed");
taskCompletedTopic.AddServiceBusSubscription("task-completed-sub");

serviceBus.RunAsEmulator();

// Add services with Service Bus reference
var invoices = builder.AddProject<Projects.Invoices>("invoices")
    .WithReference(serviceBus);

var orders = builder.AddProject<Projects.Orders>("orders")
    .WithReference(serviceBus);

var tasks = builder.AddProject<Projects.Tasks>("tasks")
    .WithReference(serviceBus);

// Add PDF Creator service
var pdfCreator = builder.AddProject<Projects.PdfCreator>("pdf-creator");

// Add n8n as Docker container
var n8n = builder.AddContainer("n8n", "n8nio/n8n:latest")
    .WithEnvironment("N8N_ENCRYPTION_KEY", "your-secure-encryption-key-here")
    .WithEnvironment("N8N_PORT", "5678")
    .WithEnvironment("WEBHOOK_URL", "https://n8n:5678/")
    .WithEnvironment("GENERIC_TIMEZONE", "Europe/Bratislava")
    .WithEnvironment("N8N_LOG_LEVEL", "debug")
    .WithEnvironment("N8N_PROTOCOL", "https")
    // Enable community nodes and install Azure Service Bus node
    .WithEnvironment("N8N_COMMUNITY_NODES_ENABLED", "true")
    .WithEnvironment("N8N_COMMUNITY_NODES", "n8n-nodes-azure-service-bus")
    // Add Service Bus connection
    .WithEnvironment("AZURE_SERVICE_BUS_CONNECTION_STRING", "{messaging.connectionString}")
    .WithBindMount("n8n-data", "/home/node/.n8n")
    .WithEndpoint("n8n", endpoint =>
    {
        endpoint.Port = 5678;
        endpoint.TargetPort = 5678;
    })
    .WithReference(serviceBus);

// Add integration service
var integration = builder.AddProject<Projects.Integration>("integration")
    .WithReference(serviceBus);
//var elsaServer = builder.AddProject<Projects.ElsaServer>("elsa-server");

//var elsaStudio = builder.AddProject<Projects.ElsaStudioBlazorWasm>("elsa-studio")
//    .WithReference(elsaServer)
//    .WithEnvironment("Backend__Url", "{services.elsa-server.bindings.https}/elsa/api");

// Add MailHog for email testing
var mailhog = builder.AddContainer("mailhog", "mailhog/mailhog")
    .WithEndpoint("smtp", endpoint =>
    {
        endpoint.Port = 1025;
        endpoint.TargetPort = 1025;
    })
    .WithEndpoint("ui", endpoint =>
    {
        endpoint.Port = 8025;
        endpoint.TargetPort = 8025;
    });

// Update n8n configuration to use MailHog
n8n.WithEnvironment("SMTP_HOST", "{services.mailhog.bindings.smtp}")
   .WithEnvironment("SMTP_PORT", "1025")
   .WithEnvironment("N8N_EMAIL_MODE", "smtp")
   .WithEnvironment("N8N_SMTP_SSL", "false")
   .WithEnvironment("N8N_SMTP_USER", "")
   .WithEnvironment("N8N_SMTP_PASS", "");

builder.Build().Run();
