using Library.Application;
using Library.Endpoint.Api;
using Library.Infrastructure;
using MGH.Core.CrossCutting.Exceptions;
using MGH.Core.CrossCutting.Logging;
using MGH.Core.Endpoint.Swagger;
using MGH.Core.Infrastructure.HealthCheck;


var builder = WebApplication.CreateBuilder(args);

RegisterLogger.CreateLoggerByConfig(builder.Configuration, builder.Host);
builder.Services.AddApiOptions(builder.Configuration);
builder.Services.AddCORS(builder.Configuration);
builder.Services.AddVersioning();
builder.Services.AddSwaggerService(builder.Configuration);
builder.Services.AddBaseMvc();
builder.Services.AddMemoryCache();
builder.Services.AddHttpContextAccessor();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddJwt(builder.Configuration);
builder.Services.AddAutoMapper(cfg => { }, AppDomain.CurrentDomain.GetAssemblies());

builder.AddApplicationServices();
builder.AddApiInfrastructuresServices();

var app = builder.Build();
app.UseRequestLocalization();
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.UseCors("CorsPolicy");
app.MapControllers();
app.UseExceptionMiddleWare();
app.UseStaticFiles();
app.UseHealthChecksEndpoints();
app.AddPrometheus();
app.UseSwaggerMiddleWare(builder.Configuration);
app.Run();