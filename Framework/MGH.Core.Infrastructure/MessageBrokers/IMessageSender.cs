using MGH.Core.Infrastructure.MessageBrokers.RabbitMQ;

namespace MGH.Core.Infrastructure.MessageBrokers;

public interface IMessageSender<T> : IDisposable
{
    void Publish(PublishModel<T> model);
    void Publish(PublishList<T> model);
}