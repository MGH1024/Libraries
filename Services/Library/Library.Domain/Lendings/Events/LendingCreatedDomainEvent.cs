using Library.Domain.Lendings.ValueObjects;
using Library.Domain.Libraries.Constant;
using MGH.Core.Domain.Buses.Commands;
using MGH.Core.Domain.Events;
using MGH.Core.Infrastructure.EventBus.RabbitMq.Attributes;

namespace Library.Domain.Lendings.Events;

[EventRouting(QueueItemProperty.CommonRoutingKey, QueueItemProperty.CommonExchangeType)]
public class LendingCreatedDomainEvent: DomainEvent, ICommand
{
    public Guid BookId { get; }
    public Guid LibraryId { get; }
    public Guid MemberId { get; }
    public LendingDate LendingDate { get; }
    public ReturnDate ReturnDate { get; }

    public LendingCreatedDomainEvent(
        Guid bookId,
        Guid libraryId,
        Guid memberId,
        LendingDate lendingDate,
        ReturnDate returnDate)
        : base(new { bookId, libraryId, memberId, lendingDate, returnDate })
    {
        BookId = bookId;
        LibraryId = libraryId;
        MemberId = memberId;
        LendingDate = lendingDate;
        ReturnDate = returnDate;
    }
}
