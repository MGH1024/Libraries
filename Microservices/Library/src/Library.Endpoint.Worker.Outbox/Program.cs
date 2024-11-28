using Application;
using Library.Endpoint.Worker.Outbox;
using Persistence;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddInfrastructuresServices(builder.Configuration);
builder.Services.AddInfrastructuresServices(builder.Configuration);
builder.Services.AddApplicationServices();
builder.RegisterApp();
