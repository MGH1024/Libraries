using Nest;
using Prometheus;
using Library.Domain;
using RabbitMQ.Client;
using System.Globalization;
using Library.Domain.Books;
using Library.Domain.Members;
using Library.Domain.Lendings;
using Library.Domain.Outboxes;
using Library.Domain.Libraries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Hosting;
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
using Microsoft.Extensions.DependencyInjection;
using MGH.Core.Infrastructure.EventBus.RabbitMq;
using MGH.Core.Infrastructure.Securities.Security;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;
using MGH.Core.Infrastructure.ElasticSearch.ElasticSearch;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MGH.Core.Infrastructure.Persistence.EF.Interceptors;
using MGH.Core.CrossCutting.Localizations.RouteConstraints;
using MGH.Core.Infrastructure.ElasticSearch.ElasticSearch.Base;
using MGH.Core.Infrastructure.ElasticSearch.ElasticSearch.Models;

namespace Library.Infrastructure;

public static class InfrastructureServiceRegistration
{
    public static void AddSettings(this HostApplicationBuilder builder)
    {
        var configBuilder = new ConfigurationBuilder()
            .AddConfiguration(builder.Configuration)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();
    }

    public static void AddWorkerInfrastructuresServices(this HostApplicationBuilder builder)
    {
        var services = builder.Services;
        var configuration = builder.Configuration;
        services.RegisterInterceptors();
        services.AddDbContextSqlServer(configuration);
        services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddRepositories();
        services.AddSecurityServices();
        services.AddTransient<IDateTime, DateTimeService>();
        services.AddAutoMapper(cfg => { }, AppDomain.CurrentDomain.GetAssemblies());
        services.AddCulture();
        services.AddAppElasticSearch(configuration);
        services.AddRabbitMqEventBus(configuration);
        services.AddFactories();
        services.UseHttpClientMetrics();
    }

    public static void AddFactories(this IServiceCollection services)
    {
        services.AddScoped<IPublicLibraryFactory, PublicLibraryFactory>();
        services.AddScoped<ILibraryPolicy, DistrictPolicy>();
    }

    public static void AddRabbitHealthChecks(
        this IHealthChecksBuilder healthBuilder, 
        IConfiguration configuration)
    {
        var defaultConnection = configuration.GetSection("RabbitMq:Connections:Default").Get<RabbitMqSettings>() 
            ?? throw new ArgumentNullException(nameof(RabbitMqOptions.Connections.Default));
        Func<IServiceProvider, IConnection> connectionFactory =
            sp =>
            {
                var factory = new ConnectionFactory
                {
                    HostName = defaultConnection.Host,
                    Port = Convert.ToInt32(defaultConnection.Port),
                    UserName = defaultConnection.Username,
                    Password = defaultConnection.Password,
                    VirtualHost = defaultConnection.VirtualHost,
                };

                return factory.CreateConnectionAsync().GetAwaiter().GetResult();
            };

        healthBuilder.AddRabbitMqHealthCheck(connectionFactory);
    }

    public static void RegisterInterceptors(this IServiceCollection services)
    {
        services.AddSingleton<AuditFieldsInterceptor>();
        services.AddSingleton<RemoveCacheInterceptor>();
        services.AddSingleton<AuditEntityInterceptor>();
        services.AddSingleton<OutboxEntityInterceptor>();
    }

    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUow, UnitOfWork>();
        services.AddScoped<IPublicLibraryRepository, PublicLibraryRepository>();
        services.AddScoped<ILendingRepository, LendingRepository>();
        services.AddScoped<IBookRepository, BookRepository>();
        services.AddScoped<IMemberRepository, MemberRepository>();
        services.AddScoped<IOutboxStore, OutboxMessageRepository>();
        services.AddScoped<IOutboxMessageRepository, OutboxMessageRepository>();
    }

    public static void AddDbContextSqlServer(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Default");
        services.AddDbContext<PublicLibraryDbContext>((sp, options) =>
        {
            options.UseSqlServer(connectionString, a => { a.EnableRetryOnFailure(); })
                .AddInterceptors(
                    sp.GetRequiredService<AuditFieldsInterceptor>(),
                    sp.GetRequiredService<RemoveCacheInterceptor>(),
                    sp.GetRequiredService<AuditEntityInterceptor>())
                .LogTo(Console.Write, LogLevel.Information);
        });
    }

    public static void AddCulture(this IServiceCollection services)
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

    public static async void AddAppElasticSearch(this IServiceCollection services, IConfiguration configuration)
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

    public static void AddDbContextPostgres(this IServiceCollection services, IConfiguration configuration)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        var connectionString = configuration.GetConnectionString("Postgres");
        services
            .AddDbContext<PublicLibraryDbContext>(options =>
                options.UseNpgsql(connectionString, a =>
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