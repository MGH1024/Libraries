using Nest;
using Prometheus;
using Library.Domain;
using System.Reflection;
using Library.Domain.Books;
using System.Globalization;
using Library.Domain.Members;
using Library.Domain.Lendings;
using Library.Domain.Outboxes;
using Library.Domain.Libraries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using MGH.Core.Infrastructure.Public;
using Library.Infrastructure.Contexts;
using MGH.Core.Infrastructure.EventBus;
using Library.Domain.Libraries.Policies;
using Microsoft.AspNetCore.Localization;
using Library.Domain.Libraries.Factories;
using Microsoft.Extensions.Configuration;
using Library.Infrastructure.Repositories;
using MGH.Core.Infrastructure.HealthCheck;
using MGH.Core.Infrastructure.Caching.Redis;
using Microsoft.Extensions.DependencyInjection;
using MGH.Core.Infrastructure.Persistence.Base;
using MGH.Core.Infrastructure.EventBus.RabbitMq;
using MGH.Core.Infrastructure.Securities.Security;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;
using MGH.Core.Infrastructure.ElasticSearch.ElasticSearch;
using MGH.Core.Infrastructure.Persistence.EF.Interceptors;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MGH.Core.CrossCutting.Localizations.RouteConstraints;
using MGH.Core.Infrastructure.ElasticSearch.ElasticSearch.Base;
using MGH.Core.Infrastructure.EventBus.RabbitMq.Configurations;
using MGH.Core.Infrastructure.Persistence.Models.Configuration;
using MGH.Core.Infrastructure.ElasticSearch.ElasticSearch.Models;

namespace Library.Infrastructure;

public static class InfrastructureServiceRegistration
{
    public static void AddInfrastructuresServices(this IServiceCollection services, IConfiguration configuration)
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
        services.AddHealthChecks(configuration);
    }

    public static void AddInfrastructuresServicesForWorkers(this IServiceCollection services, IConfiguration configuration)
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
    }

    private static void AddFactories(this IServiceCollection services)
    {
        services.AddScoped<ILibraryFactory, LibraryFactory>();
        services.AddScoped<ILibraryPolicy, DistrictPolicy>();
    }

    private static void AddHealthChecks(this IServiceCollection services, IConfiguration configuration)
    {
        var defaultConnection = configuration.GetSection("RabbitMq:Connections:Default").Get<RabbitMqConfig>() ??
                                  throw new ArgumentNullException(nameof(RabbitMqOptions.Connections.Default));

        var redisConnection = configuration.GetSection("RedisConnections:DefaultConfiguration").Get<RedisConfiguration>() ??
                                  throw new ArgumentNullException(nameof(RedisConnections.DefaultConfiguration));

        var healthBuilder = services.AddHealthChecks();
        healthBuilder.AddSqlServer(configuration["DatabaseConnection:SqlConnection"]);
        healthBuilder.AddDbContextCheck<LibraryDbContext>();
        healthBuilder.AddRabbitMqHealthCheck(defaultConnection.HealthAddress.ToString());
        healthBuilder.AddRedisHealthCheck(redisConnection.Configuration);
        services.AddHealthChecksDashboard("Library Health check");
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
        services.AddScoped<ILendingRepository, LendingRepository>();
        services.AddScoped<IBookRepository, BookRepository>();
        services.AddScoped<IMemberRepository, MemberRepository>();
        services.AddScoped<IOutboxStore, OutBoxRepository>();
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