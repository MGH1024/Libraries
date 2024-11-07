using System.Reflection;
using Application.Services.AuthService;
using Application.Services.OperationClaims;
using Application.Services.UserOperationClaims;
using Application.Services.UsersService;
using FluentValidation;
using MGH.Core.Application.Pipelines.Authorization;
using MGH.Core.Application.Pipelines.Caching;
using MGH.Core.Application.Pipelines.Logging;
using MGH.Core.Application.Pipelines.Transaction;
using MGH.Core.Application.Pipelines.Validation;
using MGH.Core.Application.Rules;
using MGH.Core.Infrastructure.ElasticSearch.ElasticSearch;
using MGH.Core.Infrastructure.ElasticSearch.ElasticSearch.Base;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Extensions;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddMediatRAndBehaviors();
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddSubClassesOfType(Assembly.GetExecutingAssembly(), typeof(BaseBusinessRules));
        services.AddSingleton<IElasticSearch, ElasticSearchService>();
        services.AddScoped<IAuthService, AuthManager>();
        services.AddScoped<IOperationClaimService, OperationClaimManager>();
        services.AddScoped<IUserOperationClaimService, UserUserOperationClaimManager>();
        services.AddScoped<IUserService, UserManager>();

        return services;
    }

    private static void AddMediatRAndBehaviors(this IServiceCollection services)
    {
        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            configuration.AddOpenBehavior(typeof(CachingBehavior<,>));
            configuration.AddOpenBehavior(typeof(LoggingBehaviour<,>));
            configuration.AddOpenBehavior(typeof(AuthorizationBehavior<,>));
            configuration.AddOpenBehavior(typeof(CacheRemovingBehavior<,>));
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