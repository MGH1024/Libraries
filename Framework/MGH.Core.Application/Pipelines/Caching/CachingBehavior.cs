﻿namespace MGH.Core.Application.Pipelines.Caching;

using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MGH.Core.Infrastructure.Cache.Redis.Services;

public class CachingBehavior<TRequest, TResponse>(ICachingService<TResponse> cacheService)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var cacheAttribute = typeof(TRequest).GetCustomAttribute<CacheAttribute>();

        if (cacheAttribute == null) return await next();
        var cacheKey = GenerateCacheKey(request);
        var res =await cacheService.GetAsync(cacheKey);
        if (res != null)
            return res;
            
        var response = await next();
            
        await cacheService.SetAsync(cacheKey, response, cacheAttribute.CacheDuration);

        return response;
    }

    private string GenerateCacheKey(TRequest request)
    {
        var keyBuilder = new StringBuilder(typeof(TRequest).Name);
        foreach (var property in typeof(TRequest).GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            var value = property.GetValue(request) ?? "null";
            keyBuilder.Append($"_{property.Name}:{value}");
        }
        return keyBuilder.ToString();
    }
}
