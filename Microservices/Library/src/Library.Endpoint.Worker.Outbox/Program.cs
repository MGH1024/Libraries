using Library.Application;
using MGH.Core.CrossCutting.Logging;
using Library.Endpoint.Worker.Outbox;
using Library.Infrastructure;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddInfrastructuresServices(builder.Configuration);
builder.Services.AddInfrastructuresServices(builder.Configuration);
builder.Services.AddApplicationServices(builder.Configuration);

RegisterLogger.CreateLoggerByConfig(builder.Configuration);
builder.Services.AddHostedService<Worker>();
var host = builder.Build();
await host.RunAsync();
