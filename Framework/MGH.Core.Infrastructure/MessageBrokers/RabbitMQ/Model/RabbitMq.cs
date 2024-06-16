namespace MGH.Core.Infrastructure.MessageBrokers.RabbitMQ.Model;

public class RabbitMq
{
    public RabbitMqConnection DataCollectorConnection { get; set; }
    public RabbitMqConnection DefaultConnection { get; set; }
}
