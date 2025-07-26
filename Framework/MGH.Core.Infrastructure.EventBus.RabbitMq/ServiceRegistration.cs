using System.Reflection;
using MGH.Core.Infrastructure.EventBus.RabbitMq.Configuration;
using MGH.Core.Infrastructure.EventBus.RabbitMq.Connection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MGH.Core.Infrastructure.EventBus.RabbitMq;

public static class ServiceRegistration
{
    public static void AddRabbitMqEventBus(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<EventBusConfig>(option => configuration.GetSection(nameof(EventBusConfig)).Bind(option));
        services.AddTransient<IEventBus, Core.EventBus>();
        services.AddTransient<IRabbitConnection, RabbitConnection>();
    }

    public static void AddEventHandlers(this IServiceCollection services, params Assembly[] assembliesToScan)
    {
        var handlerInterfaceType = typeof(IEventHandler<>);

        var types = assembliesToScan
            .SelectMany(a => a.GetTypes())
            .Where(t => !t.IsAbstract && !t.IsInterface)
            .SelectMany(t => t.GetInterfaces(), (impl, iface) => new { impl, iface })
            .Where(x =>
                x.iface.IsGenericType &&
                x.iface.GetGenericTypeDefinition() == handlerInterfaceType)
            .ToList();

        foreach (var t in types)
        {
            services.AddTransient(t.iface, t.impl);
        }
    }
}