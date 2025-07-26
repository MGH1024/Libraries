using Library.Application;
using Library.Domain.Libraries.Events;
using Library.Endpoint.Worker.Inbox;
using Library.Endpoint.Worker.Inbox.ConsumerHandlers;
using Library.Infrastructure;
using MGH.Core.CrossCutting.Logging;
using MGH.Core.Infrastructure.EventBus;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddInfrastructuresServices(builder.Configuration);
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddTransient<IEventHandler<LibraryCreatedDomainEvent2>, LibraryCreatedDomainEvent2Handler>();



RegisterLogger.CreateLoggerByConfig(builder.Configuration);
builder.Services.AddHostedService<Worker>();
var host = builder.Build();

var eventBus = host.Services.GetRequiredService<IEventBus>();
eventBus.StartConsumingAllHandlers();

await host.RunAsync();
