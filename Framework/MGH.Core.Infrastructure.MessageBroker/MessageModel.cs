namespace MGH.Core.Infrastructure.MessageBroker;

public class MessageModel<T>(BaseMessage baseMessage, T item)
{
    public BaseMessage BaseMessage { get; set; } = baseMessage;
    public T Item { get; set; } = item;
}