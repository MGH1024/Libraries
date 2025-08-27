using System.Reflection;
using FluentValidation;
using MGH.Core.Application.Pipelines.Authorization;
using MGH.Core.Application.Pipelines.Logging;
using MGH.Core.Application.Pipelines.Transaction;
using MGH.Core.Application.Pipelines.Validation;
using MGH.Core.Application.Rules;
using MGH.Core.Infrastructure.Caching;
using MGH.Core.Infrastructure.Caching.Models;
using MGH.Core.Infrastructure.ElasticSearch.ElasticSearch;
using MGH.Core.Infrastructure.ElasticSearch.ElasticSearch.Base;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Security.Application.Features.Auth.Rules;
using Security.Application.Features.Auth.Services;
using Security.Application.Features.OperationClaims.Rules;
using Security.Application.Features.OperationClaims.Services;
using Security.Application.Features.UserOperationClaims.Rules;
using Security.Application.Features.UserOperationClaims.Services;
using Security.Application.Features.Users.Rules;
using Security.Application.Features.Users.Services;

namespace Security.Application;

public static class ApplicationServiceRegistration
{
    public static void AddApplicationServices(this IServiceCollection services,IConfiguration configuration)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddMediatRAndBehaviors();
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddSubClassesOfType(Assembly.GetExecutingAssembly(), typeof(BaseBusinessRules));
        services.AddServices();
        services.AddBusinessRules();
        services.AddGeneralCachingService();
        services.AddRedis(configuration);
    }

    private static void AddBusinessRules(this IServiceCollection services)
    {
        services.AddScoped<IUserBusinessRules, UserBusinessRules>();
        services.AddScoped<IAuthBusinessRules, AuthBusinessRules>();
        services.AddScoped<IOperationClaimBusinessRules, OperationClaimBusinessRules>();
        services.AddScoped<IUserOperationClaimBusinessRules, UserOperationClaimBusinessRules>();
    }

    private static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthManager>();
        services.AddScoped<IOperationClaimService, OperationClaimManager>();
        services.AddScoped<IUserOperationClaimService, UserUserOperationClaimManager>();
        services.AddScoped<IUserService, UserManager>();
        services.AddSingleton<IElasticSearch, ElasticSearchService>();
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