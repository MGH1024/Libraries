using Library.Application;
using Library.Infrastructure;
using MGH.Core.Infrastructure.EventBus.RabbitMq;
using Library.Endpoint.Worker.Inbox.EventHandlers;

var builder = Host.CreateApplicationBuilder(args);

var configBuilder = new ConfigurationBuilder()
    .AddConfiguration(builder.Configuration)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

builder.Services.AddInfrastructuresServices(builder.Configuration);
builder.AddApplicationServices();
builder.Services.AddEventHandlers(typeof(LibraryCreatedDomainEventHandler).Assembly);

var host = builder.Build();
using var scope = host.Services.CreateScope();
scope.ServiceProvider.StartConsumingRegisteredEventHandlers();

await host.RunAsync();