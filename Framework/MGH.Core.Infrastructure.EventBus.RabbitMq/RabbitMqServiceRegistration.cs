using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MGH.Core.Infrastructure.MessageBroker.RabbitMq;

public static class RabbitMqServiceRegistration
{
    public static void AddRabbitMqEventBus(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<Model.RabbitMq>(option => configuration.GetSection(nameof(RabbitMq)).Bind(option));
        services.AddTransient<IEventBusDispatcher, RabbitMqEventBusDispatcher>();
        services.AddTransient<IRabbitMqConnection, RabbitMqConnection>();
        services.AddTransient<IRabbitMqPublisher, RabbitMqPublisher>();
    }
}