using Api;
using Application;
using Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApiService(builder.Configuration,builder.Host);
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddInfrastructuresServices(builder.Configuration);
builder.RegisterApp();
