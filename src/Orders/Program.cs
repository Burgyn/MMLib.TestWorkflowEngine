using Orders.Features;
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

// Add Orders feature
builder.Services.AddOrders(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapDefaultEndpoints(); // Health checks, etc.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Configure Orders feature
app.UseOrders();

// Map endpoints
app.MapOrders();

app.Run();
