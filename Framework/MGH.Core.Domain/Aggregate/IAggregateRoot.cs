namespace MGH.Core.Domain.Aggregate;

public interface IAggregateRoot
{
    IEnumerable<DomainEvent> Events { get; }
    void ClearEvents();
}