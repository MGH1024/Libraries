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
}