using MGH.Core.Domain.BaseEntity.Abstract;

namespace MGH.Core.Infrastructure.MessageBroker.RabbitMq;

public class RabbitMqEventBusDispatcher(IRabbitMqPublisher rabbitMqHelper) : IEventBusDispatcher
{
    public void Publish<T>(T item)  where T : IntegratedEvent
    {
        rabbitMqHelper.Publish(item);
    }
}
