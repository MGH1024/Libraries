using Library.Application;
using Library.Infrastructure;
using Library.Endpoint.Worker.Outbox;

var builder = Host.CreateApplicationBuilder(args);

builder.AddWorkerInfrastructuresServices();
builder.AddApplicationServices();

builder.Services.AddHostedService<OutBoxWorker>();
var host = builder.Build();
await host.RunAsync();
