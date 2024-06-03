namespace MGH.Core.Infrastructure.MessageBrokers.RabbitMQ;

public class PublishList<T>
{
    public PublishList(List<T> items)
    {
        Items = items;
    }

    public string RoutingKey { get; set; }
    public string  ExchangeType { get; set; }
    public string ExchangeName { get; set; }
    public string QueueName { get; set; }
    public List<T> Items { get; set; }
}