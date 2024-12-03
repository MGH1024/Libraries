namespace MGH.Core.Infrastructure.MessageBroker.RabbitMq.Model;

public class RabbitMq
{
    public RabbitMqConnection DataCollectorConnection { get; set; }
    public RabbitMqConnection DefaultConnection { get; set; }
}
