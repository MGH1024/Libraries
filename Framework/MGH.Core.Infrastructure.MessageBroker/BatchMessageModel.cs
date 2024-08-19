namespace MGH.Core.Infrastructure.MessageBroker;

public class BatchMessageModel<T>(BaseMessage baseMessage)
{
    public BaseMessage BaseMessage { get; set; } = baseMessage;
    public List<T> Items { get; set; } = new();
}