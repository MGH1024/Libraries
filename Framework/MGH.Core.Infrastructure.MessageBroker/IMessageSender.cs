namespace MGH.Core.Infrastructure.MessageBroker;

public interface IMessageSender<T> : IDisposable
{
    void Publish(PublishModel<T> model);
    void Publish(PublishList<T> model);
}