using Aspire.Hosting;
using Aspire.Hosting.ApplicationModel;
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

serviceBus.RunAsEmulator(sb =>
{
    sb.WithHttpEndpoint(targetPort: 5300, name: "sbhealthendpoint")
        .WithImageTag("1.1.2")
        .WithContainerName("servicebus")
        .WithEnvironment("SQL_WAIT_INTERVAL", "1");

    var edge = sb.ApplicationBuilder.Resources.OfType<ContainerResource>()
        .First(resource => resource.Name.EndsWith("-sqledge"));

    var annotation = edge.Annotations.OfType<ContainerImageAnnotation>().First();

    annotation.Image = "mssql/server";
    annotation.Tag = "2022-latest";
});

var sbHc = serviceBus.Resource.Annotations.OfType<HealthCheckAnnotation>().First();
serviceBus.Resource.Annotations.Remove(sbHc);

serviceBus.WithHttpHealthCheck("/health", 200, "sbhealthendpoint");

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
var n8n = builder.AddContainer("n8n", "n8nio/n8n")
    .WithEnvironment("N8N_ENCRYPTION_KEY", "your-secure-encryption-key-here")
    .WithEnvironment("N8N_PORT", "5678")
    .WithEnvironment("WEBHOOK_URL", "https://n8n:5678/")
    .WithEnvironment("GENERIC_TIMEZONE", "Europe/Bratislava")
    .WithEnvironment("N8N_LOG_LEVEL", "debug")
    .WithEnvironment("N8N_PROTOCOL", "https")
    // Enable custom nodes
    .WithEnvironment("N8N_CUSTOM_EXTENSIONS", "/home/node/.n8n/custom")
    .WithEnvironment("N8N_NODES_INCLUDE", "n8n-nodes-base,n8n-nodes-mmlib")
    .WithEnvironment("N8N_NODES_EXCLUDE_FROM_INCLUDE", "")
    .WithEnvironment("N8N_ENFORCE_SETTINGS_FILE_PERMISSIONS", "true")
    // Add Service Bus connection
    .WithEnvironment("AZURE_SERVICE_BUS_CONNECTION_STRING", "{messaging.connectionString}")
    .WithBindMount("n8n-data", "/home/node/.n8n")
    .WithBindMount(Path.Combine(Directory.GetCurrentDirectory(), "../N8n/n8n-nodes-mmlib/dist"), "/home/node/.n8n/custom/node_modules/n8n-nodes-mmlib")
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
