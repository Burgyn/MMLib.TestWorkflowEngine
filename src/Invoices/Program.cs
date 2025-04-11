using Invoices.Features;
using Aspire.Azure.Messaging.ServiceBus;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults (telemetry, health checks, etc.)
builder.AddServiceDefaults();

// Add Azure Service Bus
builder.AddAzureServiceBusClient("messaging");

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add Invoices feature
builder.Services.AddInvoices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapDefaultEndpoints(); // Health checks, etc.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configure Invoices feature
app.UseInvoices();

// Map endpoints
app.MapInvoices();

app.Run();