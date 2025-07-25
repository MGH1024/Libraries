using Prometheus;
using System.Reflection;
using System.Globalization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using MGH.Core.Infrastructure.Public;
using Security.Infrastructure.Contexts;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using MGH.Core.Infrastructure.HealthCheck;
using Security.Infrastructure.Repositories;
using MGH.Core.Infrastructure.ElasticSearch;
using Microsoft.Extensions.DependencyInjection;
using MGH.Core.Infrastructure.Persistence.Base;
using MGH.Core.Infrastructure.Securities.Security;
using Security.Infrastructure.Repositories.Security;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;
using MGH.Core.Infrastructure.Persistence.EF.Interceptors;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MGH.Core.CrossCutting.Localizations.RouteConstraints;
using MGH.Core.Infrastructure.EventBus.RabbitMq;
using MGH.Core.Infrastructure.Persistence.Models.Configuration;
using Security.Domain;
using Security.Domain.Repositories;

namespace Security.Infrastructure;

public static class InfrastructureServiceRegistration
{
    public static void AddInfrastructuresServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.RegisterInterceptors();
        services.AddDbContextSqlServer(configuration);
        services.AddDbContext<SecurityDbContext>(options => options.UseInMemoryDatabase("SecurityDbContext-InMemory"));
        services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddRepositories();
        services.AddSecurityServices();
        services.AddTransient<IDateTime, DateTimeService>();
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddCulture();
        services.AddElasticSearch(configuration);
        services.AddRabbitMqEventBus(configuration);
        services.AddPrometheus();
        services.AddInfrastructureHealthChecks<SecurityDbContext>(configuration);
    }

    private static void RegisterInterceptors(this IServiceCollection services)
    {
        services.AddSingleton<OutBoxInterceptor>();
        services.AddSingleton<AuditFieldsInterceptor>();
        services.AddSingleton<RemoveCacheInterceptor>();
        services.AddSingleton<AuditEntityInterceptor>();
    }

    private static void AddPrometheus(this IServiceCollection services)
    {
        services.UseHttpClientMetrics(); 
    }

    public static void AddPrometheus(this WebApplication app)
    {
        app.UseMetricServer();
        app.UseHttpMetrics();
    }

    private static void AddDbContextSqlServer(this IServiceCollection services, IConfiguration configuration)
    {
        var sqlConfig = configuration
            .GetSection(nameof(DatabaseConnection))
            .Get<DatabaseConnection>()
            .SqlConnection;

        services.AddDbContext<SecurityDbContext>((sp, options) =>
        {
            options.UseSqlServer(sqlConfig, a => { a.EnableRetryOnFailure(); })
                .AddInterceptors(
                    sp.GetRequiredService<OutBoxInterceptor>(),
                    sp.GetRequiredService<AuditFieldsInterceptor>(),
                    sp.GetRequiredService<RemoveCacheInterceptor>(),
                    sp.GetRequiredService<AuditEntityInterceptor>())
                .LogTo(Console.Write, LogLevel.Information);
        });
    }

    private static void AddDbContextPostgres(this IServiceCollection services, IConfiguration configuration)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        var postgresConfig = configuration
            .GetSection(nameof(DatabaseConnection))
            .Get<DatabaseConnection>()
            .PostgresConnection;
        services
            .AddDbContext<SecurityDbContext>(options =>
                options.UseNpgsql(postgresConfig, a =>
                    {
                        a.EnableRetryOnFailure();
                        //a.MigrationsAssembly("Library.Api");
                    })
                    .AddInterceptors()
                    .LogTo(Console.Write, LogLevel.Information));
    }

    private static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUow, UnitOfWork>();
        services.AddScoped<IOperationClaimRepository, OperationClaimRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<IUserOperationClaimRepository, UserOperationClaimRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped(typeof(ITransactionManager<>), typeof(TransactionManager<>));
    }

    private static void AddCulture(this IServiceCollection services)
    {
        var supportedCultures = CultureInfo.GetCultures(CultureTypes.AllCultures);

        services
            .Configure<RouteOptions>(routeOptions =>
            {
                if (!routeOptions.ConstraintMap.ContainsKey(nameof(CultureRouteConstraint)))
                {
                    routeOptions.ConstraintMap.Add(nameof(CultureRouteConstraint), typeof(CultureRouteConstraint));
                }
            })
            .Configure<RequestLocalizationOptions>(requestLocalizationOptions =>
            {
                requestLocalizationOptions.DefaultRequestCulture =
                    new RequestCulture(CultureInfo.GetCultureInfo("en-US"));
                requestLocalizationOptions.SupportedCultures = supportedCultures;
                requestLocalizationOptions.SupportedUICultures = supportedCultures;
                requestLocalizationOptions.RequestCultureProviders.Insert(0, new CultureRequestCultureProvider());
            })
            .AddLocalization(opt => { opt.ResourcesPath = "Resources"; });
    }
   
}