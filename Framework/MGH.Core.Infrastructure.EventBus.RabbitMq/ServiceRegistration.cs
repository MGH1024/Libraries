using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MGH.Core.Infrastructure.MessageBroker.RabbitMq.Concrete;
using MGH.Core.Infrastructure.MessageBroker.RabbitMq.Abstracts;

namespace MGH.Core.Infrastructure.MessageBroker.RabbitMq;

public static class ServiceRegistration
{
    public static void AddRabbitMqEventBus(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<Model.RabbitMq>(option => configuration.GetSection(nameof(RabbitMq)).Bind(option));
        services.AddTransient<IEventBusDispatcher, EventBusDispatcher>();
        services.AddTransient<IRabbitMqConnection, Connection>();
    }
}