using System.Globalization;
using System.Reflection;
using Library.Domain;
using Library.Domain.Entities.Libraries;
using Library.Domain.Entities.Libraries.Factories;
using Library.Domain.Entities.Libraries.Policies;
using Library.Infrastructure.Contexts;
using Library.Infrastructure.Repositories;
using MGH.Core.CrossCutting.Localizations.RouteConstraints;
using MGH.Core.Infrastructure.ElasticSearch.ElasticSearch;
using MGH.Core.Infrastructure.ElasticSearch.ElasticSearch.Base;
using MGH.Core.Infrastructure.ElasticSearch.ElasticSearch.Models;
using MGH.Core.Infrastructure.HealthCheck;
using MGH.Core.Infrastructure.MessageBroker.RabbitMq;
using MGH.Core.Infrastructure.Persistence.Base;
using MGH.Core.Infrastructure.Persistence.EF.Interceptors;
using MGH.Core.Infrastructure.Persistence.Models.Configuration;
using MGH.Core.Infrastructure.Public;
using MGH.Core.Infrastructure.Securities.Security;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Nest;
using Prometheus;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace Library.Infrastructure;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructuresServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.RegisterInterceptors();
        services.AddDbContextSqlServer(configuration);
        services.AddDbContext<LibraryDbContext>(options => options.UseInMemoryDatabase("LibraryDbContext-InMemory"));
        services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddRepositories();
        services.AddSecurityServices();
        services.AddTransient<IDateTime, DateTimeService>();
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddCulture();
        services.AddElasticSearch(configuration);
        services.AddRabbitMqEventBus(configuration);
        services.AddFactories();
        services.AddPrometheus();
        services.AddInfrastructureHealthChecks<LibraryDbContext>(configuration);
        return services;
    }

    private static void AddFactories(this IServiceCollection services)
    {
        services.AddScoped<ILibraryFactory, LibraryFactory>();
        services.AddScoped<ILibraryPolicy, DistrictPolicy>();
    }

    private static void AddPrometheus(this IServiceCollection services)
    {
        services.UseHttpClientMetrics();
    }

    private static void RegisterInterceptors(this IServiceCollection services)
    {
        services.AddSingleton<OutBoxInterceptor>();
        services.AddSingleton<AuditFieldsInterceptor>();
        services.AddSingleton<RemoveCacheInterceptor>();
        services.AddSingleton<AuditEntityInterceptor>();
    }

    private static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUow, UnitOfWork>(); 
        services.AddScoped<ILibraryRepository, LibraryRepository>();
        services.AddScoped<IOutBoxRepository, OutBoxRepository>();
        services.AddScoped(typeof(ITransactionManager<>), typeof(TransactionManager<>));
    }

    private static void AddDbContextSqlServer(this IServiceCollection services, IConfiguration configuration)
    {
        var sqlConfig = configuration
            .GetSection(nameof(DatabaseConnection))
            .Get<DatabaseConnection>()
            .SqlConnection;

        services.AddDbContext<LibraryDbContext>((sp, options) =>
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

    private static async void AddElasticSearch(this IServiceCollection services, IConfiguration configuration)
    {
        const string configurationSection = "ElasticSearchConfig";
        var setting =
            configuration.GetSection(configurationSection).Get<ElasticSearchConfig>()
            ?? throw new NullReferenceException($"\"{configurationSection}\" " +
                                                $"section cannot found in configuration.");

        var connectionSettings = new ConnectionSettings(new Uri(setting.ConnectionString));
        var client = new ElasticClient(connectionSettings);
        services.AddSingleton(client);
        services.AddSingleton<IElasticSearch, ElasticSearchService>(x => new ElasticSearchService(client));
        foreach (var i in setting.Indices)
        {
            if (!(await client.Indices.ExistsAsync(i.IndexName)).Exists)
            {
                await client.Indices.CreateAsync(i.IndexName, selector: se =>
                    se.Settings(a => a.NumberOfReplicas(i.ReplicaCount)
                            .NumberOfShards(i.ShardNumber))
                        .Aliases(x => x.Alias(i.AliasName))
                );
            }
        }
    }

    private static void AddDbContextPostgres(this IServiceCollection services, IConfiguration configuration)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        var postgresConfig = configuration
            .GetSection(nameof(DatabaseConnection))
            .Get<DatabaseConnection>()
            .PostgresConnection;
        services
            .AddDbContext<LibraryDbContext>(options =>
                options.UseNpgsql(postgresConfig, a =>
                    {
                        a.EnableRetryOnFailure();
                        //a.MigrationsAssembly("Library.Api");
                    })
                    .AddInterceptors()
                    .LogTo(Console.Write, LogLevel.Information));
    }
    
    public static void AddPrometheus(this WebApplication app)
    {
        app.UseMetricServer();
        app.UseHttpMetrics();
    }
    
}