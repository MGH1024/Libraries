namespace MGH.Core.Infrastructure.MessageBrokers.RabbitMQ;

public class RabbitMq
{
    public RabbitMqConnection DataCollectorConnection { get; set; }
    public RabbitMqConnection DefaultConnection { get; set; }
}
