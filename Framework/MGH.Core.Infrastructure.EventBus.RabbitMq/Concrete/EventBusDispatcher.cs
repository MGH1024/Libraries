using MGH.Core.Domain.BaseEntity.Abstract;
using MGH.Core.Infrastructure.MessageBroker.RabbitMq.Abstracts;

namespace MGH.Core.Infrastructure.MessageBroker.RabbitMq.Concrete;

public class EventBusDispatcher(IRabbitMqPublisher rabbitMqHelper) : IEventBusDispatcher
{
    public void Publish<T>(T item)  where T : IntegratedEvent
    {
        rabbitMqHelper.Publish(item);
    }
}
