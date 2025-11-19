using Library.Application;
using Library.Endpoint.Api;
using Library.Infrastructure;

var builder = WebApplication.CreateBuilder(args);



var configBuilder = new ConfigurationBuilder()
            .AddConfiguration(builder.Configuration) 
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();


builder.Services.AddApiService(builder.Configuration,builder.Host);
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddInfrastructuresServices(builder.Configuration);
builder.RegisterApp();