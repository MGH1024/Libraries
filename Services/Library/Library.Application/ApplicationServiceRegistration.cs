using FluentValidation;
using System.Reflection;
using MGH.Core.Application.Rules;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using MGH.Core.Infrastructure.Caching;
using MGH.Core.Application.Pipelines.Logging;
using MGH.Core.Infrastructure.Caching.Models;
using Microsoft.Extensions.DependencyInjection;
using MGH.Core.Application.Pipelines.Validation;
using MGH.Core.Application.Pipelines.Transaction;
using MGH.Core.Application.Pipelines.Authorization;
using MGH.Core.Infrastructure.ElasticSearch.ElasticSearch;
using MGH.Core.Infrastructure.ElasticSearch.ElasticSearch.Base;
using Library.Application.Features.PublicLibraries.Commands.Add;

namespace Library.Application;

public static class ApplicationServiceRegistration
{
    public static void AddApplicationServices(this WebApplicationBuilder builder)
    {
        var services = builder.Services;
        var configuration = builder.Configuration;
        services.AddAutoMapper(cfg => { }, AppDomain.CurrentDomain.GetAssemblies());
        services.AddMediatRAndBehaviors(builder.Environment);
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddSubClassesOfType(Assembly.GetExecutingAssembly(), typeof(BaseBusinessRules));
        services.AddRedis(configuration);
        services.AddGeneralCachingService();
        services.AddServices();
    }

    public static void AddApplicationServices(this HostApplicationBuilder builder)
    {
        var services = builder.Services;
        var configuration = builder.Configuration;
        services.AddAutoMapper(cfg => { }, AppDomain.CurrentDomain.GetAssemblies());
        services.AddMediatRAndBehaviors(builder.Environment);
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddSubClassesOfType(Assembly.GetExecutingAssembly(), typeof(BaseBusinessRules));
        services.AddRedis(configuration);
        services.AddGeneralCachingService();
        services.AddServices();
    }

    private static void AddServices(this IServiceCollection services)
    {
        services.AddSingleton<IElasticSearch, ElasticSearchService>();
    }

    private static void AddMediatRAndBehaviors(this IServiceCollection services, IHostEnvironment environment)
    {
        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            configuration.RegisterServicesFromAssembly(typeof(AddCommand).Assembly);
            configuration.AddOpenBehavior(typeof(CachingBehavior<,>));
            configuration.AddOpenBehavior(typeof(LoggingBehaviour<,>));
            if (environment.EnvironmentName != "Development")
                configuration.AddOpenBehavior(typeof(AuthorizationBehavior<,>));
            configuration.AddOpenBehavior(typeof(TransactionScopeBehavior<,>));
            configuration.AddOpenBehavior(typeof(RequestValidationBehavior<,>));
        });
    }

    private static void AddSubClassesOfType(this IServiceCollection services, Assembly assembly,
        Type type, Func<IServiceCollection, Type, IServiceCollection> addWithLifeCycle = null)
    {
        var types = assembly.GetTypes().Where(t => t.IsSubclassOf(type) && type != t).ToList();
        foreach (Type item in types)
            if (addWithLifeCycle == null)
                services.AddScoped(item);
            else
                addWithLifeCycle(services, type);
    }
}