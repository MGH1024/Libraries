using Application;
using Infrastructures;
using Library.Worker.Outbox;
using Persistence;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddPersistenceService(builder.Configuration);
builder.Services.AddInfrastructuresServices(builder.Configuration);
builder.Services.AddApplicationServices();
builder.RegisterApp();
