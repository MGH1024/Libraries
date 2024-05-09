using Api;
using Serilog;
using Persistence;
using Application;
using Infrastructures;
using MGH.Core.CrossCutting.Exceptions;

//var configurationBuilder = new ConfigurationBuilder();
//ApiServiceRegistration.CreateLoggerByConfig(configurationBuilder.GetLogConfig());

var builder = WebApplication.CreateBuilder(args);
Log.Information("web starting up ...");
builder.Services.AddControllers();
builder.Services.AddPersistenceService(builder.Configuration);
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructuresServices(builder.Configuration);
builder.Services.AddHttpContextAccessor();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddMemoryCache();
builder.AddSwagger();
builder.AddBaseMvc();
builder.AddCors();
builder.Host.UseSerilog();
var app = builder.Build();
app.RegisterApp();
app.UseMiddleware<ExceptionMiddleware>();
app.Run();