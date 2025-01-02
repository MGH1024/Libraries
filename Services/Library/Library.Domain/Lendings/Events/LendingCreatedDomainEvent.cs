using Library.Domain.Lendings.ValueObjects;
using Library.Domain.Libraries.Constant;
using MGH.Core.Domain.Buses.Commands;
using MGH.Core.Domain.Events;
using MGH.Core.Infrastructure.MessageBroker.RabbitMq.Attributes;

namespace Library.Domain.Lendings.Events;

[BaseMessage(
    QueueItemProperty.CommonRoutingKey,
    QueueItemProperty.CommonExchangeType,
    QueueItemProperty.CommonExchangeName,
    QueueItemProperty.CommonQueueName
)]
public class LendingCreatedDomainEvent(Guid bookId, Guid libraryId, Guid memberId, LendingDate lendingDate,ReturnDate returnDate): DomainEvent, ICommand
{
    public Guid BookId => bookId;
    public Guid LibraryId => libraryId;
    public Guid MemberId => memberId;
    public LendingDate LendingDate => lendingDate;
    public ReturnDate ReturnDate => returnDate;
}