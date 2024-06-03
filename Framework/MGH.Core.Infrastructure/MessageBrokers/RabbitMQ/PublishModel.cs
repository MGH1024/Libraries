namespace MGH.Core.Infrastructure.MessageBrokers.RabbitMQ;

public class PublishModel<T>
{
    public string RoutingKey { get; set; }
    public string ExchangeName { get; set; }
    public string ExchangeType { get; set; }
    public string QueueName { get; set; }
    public T Item { get; set; }
}