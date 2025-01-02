using MGH.Core.Domain.Events;

namespace MGH.Core.Domain.BaseModels;

public interface IAggregate<T> : IAggregate, IEntity<T>
{
}

public interface IAggregate : IEntity
{
    IReadOnlyList<DomainEvent> DomainEvents { get; }
    IEvent[] ClearDomainEvents();
}