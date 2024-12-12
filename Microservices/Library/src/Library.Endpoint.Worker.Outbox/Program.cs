using Application;
using Persistence;
using MGH.Core.CrossCutting.Logging;
using Library.Endpoint.Worker.Outbox;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddInfrastructuresServices(builder.Configuration);
builder.Services.AddInfrastructuresServices(builder.Configuration);
builder.Services.AddApplicationServices();

RegisterLogger.CreateLoggerByConfig(builder.Configuration);
builder.Services.AddHostedService<Worker>();
var host = builder.Build();
await host.RunAsync();
