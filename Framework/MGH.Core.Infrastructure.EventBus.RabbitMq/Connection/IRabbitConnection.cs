using RabbitMQ.Client;

namespace MGH.Core.Infrastructure.EventBus.RabbitMq.Connection;

public interface IRabbitConnection :IDisposable
{
    void ConnectService();
    IModel GetChannel();
}