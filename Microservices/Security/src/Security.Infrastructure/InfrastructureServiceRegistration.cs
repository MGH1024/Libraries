﻿using System.Globalization;
using System.Reflection;
using Domain;
using Domain.Repositories;
using MGH.Core.CrossCutting.Localizations.RouteConstraints;
using MGH.Core.Infrastructure.ElasticSearch.ElasticSearch;
using MGH.Core.Infrastructure.ElasticSearch.ElasticSearch.Base;
using MGH.Core.Infrastructure.ElasticSearch.ElasticSearch.Models;
using MGH.Core.Infrastructure.MessageBroker;
using MGH.Core.Infrastructure.MessageBroker.RabbitMq;
using MGH.Core.Infrastructure.MessageBroker.RabbitMq.Model;
using MGH.Core.Infrastructure.Public;
using MGH.Core.Infrastructure.Securities.Security;
using MGH.Core.Persistence.Models.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Nest;
using Persistence.Contexts;
using Persistence.Repositories;
using Persistence.Repositories.Security;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace Persistence;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructuresServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        var sqlConfig = configuration
            .GetSection(nameof(DatabaseConnection))
            .Get<DatabaseConnection>()
            .SqlConnection;

        services.AddDbContext<SecurityDbContext>(options =>
            options.UseSqlServer(sqlConfig, a =>
                {
                    a.EnableRetryOnFailure();
                    //a.MigrationsAssembly("Library.Api");
                })
                .AddInterceptors()
                .LogTo(Console.Write, LogLevel.Information));


        #region postgres

        // AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        // var postgresConfig = configuration
        //     .GetSection(nameof(DatabaseConnection))
        //     .Get<DatabaseConnection>()
        //     .PostgresConnection;
        // services
        //     .AddDbContext<LibraryDbContext>(options =>
        //         options.UseNpgsql(postgresConfig, a =>
        //             {
        //                 a.EnableRetryOnFailure();
        //                 //a.MigrationsAssembly("Library.Api");
        //             })
        //             .AddInterceptors()
        //             .LogTo(Console.Write, LogLevel.Information));

        #endregion


        services.AddDbContext<SecurityDbContext>(options => options.UseInMemoryDatabase("LibraryDbContext-InMemory"));
        services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddSecurityRepositories();
        services.AddSecurityServices();
        services.AddScoped<IUow, UnitOfWork>();
        services.AddTransient<IDateTime, DateTimeService>();
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddCulture();
        services.AddElasticSearch(configuration);
        services.AddRabbitMq(configuration);
        services.AddInfrastructureHealthChecks(configuration);
        return services;
    }

    private static void AddSecurityRepositories(this IServiceCollection services)
    {
        services.AddScoped<IOperationClaimRepository, OperationClaimRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<IUserOperationClaimRepository, UserOperationClaimRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
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

    private static void AddRabbitMq(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RabbitMq>(option => configuration.GetSection(nameof(RabbitMq)).Bind(option));
        services.AddTransient(typeof(IMessageSender<>), typeof(RabbitMqService<>));
    }

    private static void AddInfrastructureHealthChecks(this IServiceCollection services, IConfiguration configuration)
    {
        var sqlConnectionString = configuration.GetSection(nameof(DatabaseConnection))
                                      .GetValue<string>("SqlConnection") ??
                                  throw new ArgumentNullException(nameof(DatabaseConnection.SqlConnection));

        var defaultConnection = configuration.GetSection("RabbitMq:DefaultConnection").Get<RabbitMqConnection>()
                                ?? throw new ArgumentNullException(nameof(RabbitMq.DefaultConnection));

        services.AddHealthChecks()
            .AddSqlServer(sqlConnectionString)
            .AddDbContextCheck<SecurityDbContext>()
            .AddRabbitMQ(defaultConnection.HealthAddress);

        services.AddHealthChecksUI(setup =>
            {
                setup.SetEvaluationTimeInSeconds(10); // Set the evaluation time for health checks
                setup.MaximumHistoryEntriesPerEndpoint(60); // Set maximum history of health checks
                setup.SetApiMaxActiveRequests(1); // Set maximum API request concurrency
                setup.AddHealthCheckEndpoint("Health Check API", "/api/health"); // Map your health check API
            })
            .AddInMemoryStorage();
    }
}