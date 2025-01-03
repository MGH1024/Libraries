using FluentValidation;
using System.Reflection;
using MGH.Core.Application.Rules;
using Microsoft.Extensions.Configuration;
using MGH.Core.Application.Pipelines.Logging;
using Microsoft.Extensions.DependencyInjection;
using MGH.Core.Application.Pipelines.Validation;
using MGH.Core.Application.Pipelines.Transaction;
using MGH.Core.Application.Pipelines.Authorization;
using Library.Application.Features.Libraries.Rules;
using MGH.Core.Infrastructure.Caching;
using MGH.Core.Infrastructure.Caching.Models;
using MGH.Core.Infrastructure.ElasticSearch.ElasticSearch;
using MGH.Core.Infrastructure.ElasticSearch.ElasticSearch.Base;

namespace Library.Application;

public static class ApplicationServiceRegistration
{
    public static void AddApplicationServices(this IServiceCollection services,IConfiguration configuration)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddMediatRAndBehaviors();
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddSubClassesOfType(Assembly.GetExecutingAssembly(), typeof(BaseBusinessRules));
        services.AddBusinessRule();
        services.AddRedis(configuration);
        services.AddGeneralCachingService();
        services.AddGeneralCachingService();
        services.AddServices();
    }

    private static void AddServices(this IServiceCollection services)
    {
        services.AddSingleton<IElasticSearch, ElasticSearchService>();
    }

    private static void AddBusinessRule(this IServiceCollection services)
    {
        services.AddTransient<ILibraryBusinessRules, LibraryBusinessRules>();
    }

    private static void AddMediatRAndBehaviors(this IServiceCollection services)
    {
        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            configuration.AddOpenBehavior(typeof(CachingBehavior<,>));
            configuration.AddOpenBehavior(typeof(LoggingBehaviour<,>));
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