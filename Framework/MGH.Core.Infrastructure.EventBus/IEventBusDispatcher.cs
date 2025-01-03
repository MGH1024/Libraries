using MGH.Core.Domain.Events;

namespace MGH.Core.Infrastructure.EventBus;

public interface IEventBusDispatcher 
{
    void Publish<T>(T model) where T : IEvent ;
    void Publish<T>(IEnumerable<T> models) where T : IEvent;
}