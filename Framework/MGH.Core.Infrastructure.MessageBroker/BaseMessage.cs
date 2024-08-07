namespace MGH.Core.Infrastructure.MessageBroker;

public class BaseMessage
{
    public string RoutingKey { get; set; }
    public string ExchangeName { get; set; }
    public string ExchangeType { get; set; }
    public string QueueName { get; set; }
}