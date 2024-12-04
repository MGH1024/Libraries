using RabbitMQ.Client;

namespace MGH.Core.Infrastructure.MessageBroker.RabbitMq.Abstracts;

public interface IRabbitMqConnection :IDisposable
{
    void ConnectService();
    IModel GetChannel();
}