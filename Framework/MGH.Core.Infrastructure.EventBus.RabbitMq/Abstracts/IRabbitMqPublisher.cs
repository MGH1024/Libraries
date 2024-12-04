namespace MGH.Core.Infrastructure.MessageBroker.RabbitMq.Abstracts;

public interface IRabbitMqPublisher
{
    void Publish<T>(T model);
}