using Library.Application;
using Library.Infrastructure;
using MGH.Core.CrossCutting.Logging;
using Library.Endpoint.Worker.Outbox;
using Library.Domain.Libraries.Events;
using MGH.Core.Infrastructure.EventBus;
using Library.Endpoint.Worker.Outbox.ConsumerHandlers;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddInfrastructuresServices(builder.Configuration);
builder.Services.AddInfrastructuresServices(builder.Configuration);
builder.Services.AddApplicationServices(builder.Configuration);

builder.Services.AddTransient<IEventHandler<LibraryCreatedDomainEvent2>, LibraryCreatedDomainEvent2Handler>();


RegisterLogger.CreateLoggerByConfig(builder.Configuration);
builder.Services.AddHostedService<Worker>();
var host = builder.Build();
await host.RunAsync();
