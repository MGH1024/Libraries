namespace MGH.Core.Infrastructure.MessageBroker;

public interface IMessageSender<T> : IDisposable
{
    void Publish(MessageModel<T> model);
    void Publish(BatchMessageModel<T> model);
}