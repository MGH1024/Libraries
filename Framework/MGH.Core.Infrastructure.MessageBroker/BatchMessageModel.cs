namespace MGH.Core.Infrastructure.MessageBroker;

public class BatchMessageModel<T>
{
    public BatchMessageModel()
    {
        Items = new List<T>();
    }

    public BaseMessage BaseMessage { get; set; }
    public List<T> Items { get; set; }
}