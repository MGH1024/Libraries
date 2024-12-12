using MGH.Core.Domain.BaseEntity.Abstract.Events;

namespace MGH.Core.Domain.BaseEntity.Abstract;

public interface IAggregateRoot
{
    void ClearEvents();
    IReadOnlyCollection<DomainEvent> Events { get; }
}