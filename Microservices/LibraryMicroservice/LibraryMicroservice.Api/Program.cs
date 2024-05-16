using Api;
using Persistence;
using Application;
using Infrastructures;
using MGH.Core.CrossCutting.Exceptions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
//log
builder.CreateLoggerByConfig();
//log
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
Log.Information("web starting up ...");