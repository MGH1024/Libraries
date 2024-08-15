using Api;
using Application;
using Application.Extensions;
using Infrastructures;
using Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApiService(builder.Configuration,builder.Host);
builder.Services.AddPersistenceService(builder.Configuration);
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructuresServices(builder.Configuration);
builder.RegisterApp();