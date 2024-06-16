using MGH.Core.Infrastructure.MessageBrokers.RabbitMQ.Model;

namespace MGH.Core.Infrastructure.MessageBrokers.Base;

public interface IMessageSender<T> : IDisposable
{
    void Publish(PublishModel<T> model);
    void Publish(PublishList<T> model);
}