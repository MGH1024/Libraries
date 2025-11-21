using MGH.Core.Domain.Events;
using Library.Domain.Lendings.ValueObjects;
using MGH.Core.Application.Buses.Commands;

namespace Library.Domain.Lendings.Events;

public class LendingCreatedDomainEvent: DomainEvent, ICommand
{
    public Guid BookId { get; }
    public Guid LibraryId { get; }
    public Guid MemberId { get; }
    public ReturnDate ReturnDate { get; }
    public LendingDate LendingDate { get; }

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
