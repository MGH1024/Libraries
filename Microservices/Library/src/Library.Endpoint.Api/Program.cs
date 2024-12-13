using Library.Application;
using Library.Endpoint.Api;
using Library.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApiService(builder.Configuration,builder.Host);
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddInfrastructuresServices(builder.Configuration);
builder.RegisterApp();