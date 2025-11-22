using Library.Application;
using Library.Infrastructure;
using MGH.Core.CrossCutting.Logging;
using MGH.Core.Infrastructure.EventBus.RabbitMq;
using Library.Endpoint.Worker.Inbox.EventHandlers;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddInfrastructuresServicesForWorkers(builder.Configuration);
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddEventHandlers(typeof(LibraryCreatedDomainEventHandler).Assembly);

RegisterLogger.CreateLoggerByConfig(builder.Configuration);
var host = builder.Build();

using var scope = host.Services.CreateScope();
scope.ServiceProvider.StartConsumingRegisteredEventHandlers();


await host.RunAsync();