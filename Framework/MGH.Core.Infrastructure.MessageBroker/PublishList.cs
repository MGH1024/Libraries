namespace MGH.Core.Infrastructure.MessageBroker;

public class PublishList<T>
{
    public PublishList()
    {
        Items = new List<T>();
    }

    public string RoutingKey { get; set; }
    public string  ExchangeType { get; set; }
    public string ExchangeName { get; set; }
    public string QueueName { get; set; }
    public List<T> Items { get; set; }
}