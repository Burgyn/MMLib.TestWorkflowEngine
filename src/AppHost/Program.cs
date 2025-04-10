using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

// Add services
var invoices = builder.AddProject<Projects.Invoices>("invoices");
var orders = builder.AddProject<Projects.Orders>("orders");
var tasks = builder.AddProject<Projects.Tasks>("tasks");

builder.Build().Run();
