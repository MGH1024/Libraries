using Library.Application;
using Library.Infrastructure;

var builder = Host.CreateApplicationBuilder(args);

builder.AddSettings();
builder.AddWorkerInfrastructuresServices();
builder.AddApplicationServices();

var host = builder.Build();
await host.RunAsync();