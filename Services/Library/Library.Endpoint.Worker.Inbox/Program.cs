using Library.Application;
using Library.Infrastructure;
using Library.Endpoint.Worker.Inbox;
using MGH.Core.CrossCutting.Logging;
using MGH.Core.Infrastructure.EventBus.RabbitMq;
using Library.Endpoint.Worker.Inbox.EventHandlers;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddInfrastructuresServicesForWorkers(builder.Configuration);
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddEventHandlers(typeof(LibraryCreatedDomainEvent2Handler).Assembly);



RegisterLogger.CreateLoggerByConfig(builder.Configuration);
builder.Services.AddHostedService<InboxWorker>();
var host = builder.Build();

host.Services.StartConsumingRegisteredEventHandlers();

await host.RunAsync();
