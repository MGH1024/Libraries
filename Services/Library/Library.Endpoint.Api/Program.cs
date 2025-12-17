using FluentValidation;
using HealthChecks.UI.Client;
using Library.Application;
using Library.Endpoint.Api;
using Library.Infrastructure;
using Library.Infrastructure.Contexts;
using MGH.Core.Application.Rules;
using MGH.Core.CrossCutting.Exceptions;
using MGH.Core.CrossCutting.Logging;
using MGH.Core.Infrastructure.Caching;
using MGH.Core.Infrastructure.ElasticSearch.ElasticSearch;
using MGH.Core.Infrastructure.ElasticSearch.ElasticSearch.Base;
using MGH.Core.Infrastructure.Public;
using MGH.Core.Infrastructure.Securities.Security;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.OpenApi;
using Prometheus;
using System.Reflection;


var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;
var envName = builder.Environment.EnvironmentName;

RegisterLogger.CreateLoggerByConfig(configuration, builder.Host);
services.AddApiOptions(configuration);
services.AddCORS(configuration);
services.AddVersioning();
services.AddSwaggerService(configuration);
services.AddBaseMvc();
services.AddMemoryCache();
services.AddHttpContextAccessor();
services.AddEndpointsApiExplorer();
services.AddJwt(configuration);

services.AddAutoMapper(cfg => { }, AppDomain.CurrentDomain.GetAssemblies());
services.AddMediatRAndBehaviors(builder.Environment);
services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
services.AddSubClassesOfType(Assembly.GetExecutingAssembly(), typeof(BaseBusinessRules));
services.AddRedis(configuration);
services.AddGeneralCachingService();
services.AddSingleton<IElasticSearch, ElasticSearchService>();

services.RegisterInterceptors();
services.AddDbContextSqlServer(configuration);
services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
services.AddRepositories();
services.AddSecurityServices();
services.AddTransient<IDateTime, DateTimeService>();
services.AddCulture();
services.AddAppElasticSearch(configuration);
services.AddFactories();
services.UseHttpClientMetrics();

var sqlConnection = configuration.GetConnectionString("default");
var healthBuilder = services.AddHealthChecks()
    .AddSqlServer(sqlConnection)
    .AddDbContextCheck<PublicLibraryDbContext>(sqlConnection);

services.AddHealthChecksUI(setup =>
{
    setup.SetEvaluationTimeInSeconds(10);
    setup.MaximumHistoryEntriesPerEndpoint(60);
    setup.SetApiMaxActiveRequests(1);
    setup.AddHealthCheckEndpoint($"Library Health check {envName}",
        "/health");
}).AddSqlServerStorage(sqlConnection);

var app = builder.Build();
app.UseRequestLocalization();
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.UseCors("CorsPolicy");
app.MapControllers();
app.UseExceptionMiddleWare();
app.UseStaticFiles();
app.AddPrometheus();
app.UseSwaggerMiddleWare(builder.Configuration);


app.MapHealthChecks("/health/live", new HealthCheckOptions
{
    Predicate = _ => false
});

app.MapHealthChecks("/health/ready", new HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("ready"),
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.MapHealthChecksUI(options =>
{
    options.UIPath = "/health-ui";
    options.AddCustomStylesheet("healthCheck.css");
});

app.Run();
