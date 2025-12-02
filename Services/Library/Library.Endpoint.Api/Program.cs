using Library.Application;
using Library.Endpoint.Api;
using Library.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.AddApiService();
builder.AddApplicationServices();
builder.AddApiInfrastructuresServices();
builder.RegisterApp();