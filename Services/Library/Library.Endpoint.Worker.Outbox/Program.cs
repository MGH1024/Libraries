using Library.Application;
using Library.Infrastructure;
using MGH.Core.CrossCutting.Logging;
using Library.Endpoint.Worker.Outbox;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddInfrastructuresServices(builder.Configuration);
builder.AddApplicationServices();


RegisterLogger.CreateLoggerByConfig(builder.Configuration);
builder.Services.AddHostedService<OutBoxWorker>();
var host = builder.Build();
await host.RunAsync();
