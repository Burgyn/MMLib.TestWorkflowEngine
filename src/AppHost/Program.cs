using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

// Add Azure Service Bus emulator
var serviceBus = builder.AddContainer("servicebus", "mcr.microsoft.com/azure-messaging/servicebus-emulator")
    .WithEndpoint(name: "servicebus", targetPort: 9354, port: 9354)
    .WithEndpoint(name: "admin", targetPort: 9355, port: 9355)
    .WithEndpoint(name: "amqp", targetPort: 5671, port: 5671)
    .WithEnvironment("SERVICEBUS_QUEUE_NAME", "events")
    .WithEnvironment("ACCEPT_EULA", "Y");

// Add connection string for services
builder.Configuration["ConnectionStrings:ServiceBus"] = "Endpoint=sb://localhost:5671;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=123456";

// Add services
var invoices = builder.AddProject<Projects.Invoices>("invoices");

var orders = builder.AddProject<Projects.Orders>("orders");

var tasks = builder.AddProject<Projects.Tasks>("tasks");

builder.Build().Run();
