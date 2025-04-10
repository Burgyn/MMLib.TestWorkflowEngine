var builder = DistributedApplication.CreateBuilder(args);

// Set Aspire store path
builder.Configuration["Aspire:Store:Path"] = Path.Combine(Directory.GetCurrentDirectory(), "aspire-store");

// Add Azure Service Bus with emulator
var serviceBus = builder.AddAzureServiceBus("messaging");

// Configure domain events topic with subscriptions per service
var eventsTopic = serviceBus.AddServiceBusTopic("domain-events");
eventsTopic.AddServiceBusSubscription("orders-subscription");
eventsTopic.AddServiceBusSubscription("invoices-subscription");
eventsTopic.AddServiceBusSubscription("tasks-subscription");

serviceBus.RunAsEmulator();

// Add services with Service Bus reference
var invoices = builder.AddProject<Projects.Invoices>("invoices")
    .WithReference(serviceBus);

var orders = builder.AddProject<Projects.Orders>("orders")
    .WithReference(serviceBus);

var tasks = builder.AddProject<Projects.Tasks>("tasks")
    .WithReference(serviceBus);

builder.Build().Run();
