namespace MGH.Core.Infrastructure.MessageBroker.RabbitMq;

public interface IRabbitMqPublisher
{
    void Publish<T>(T model);
}