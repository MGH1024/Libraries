using MGH.Core.Domain.BaseEntity.Abstract;

namespace MGH.Core.Infrastructure.MessageBroker;

public interface IEventBusDispatcher : IDisposable
{
    void PublishAsync<T>(T model) where T : IntegratedEvent;
}