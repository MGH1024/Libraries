using RabbitMQ.Client;

namespace MGH.Core.Infrastructure.MessageBroker.RabbitMq;

public interface IRabbitMqConnection :IDisposable
{
    void ConnectService();
    IModel GetChannel();
}