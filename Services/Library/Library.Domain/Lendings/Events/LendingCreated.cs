using System;
using MGH.Core.Domain.Events;
using Library.Domain.Lendings.ValueObjects;

namespace Library.Domain.Lendings.Events;

public sealed class LendingCreated : DomainEvent
{
    public Guid BookId { get; }
    public Guid LibraryId { get; }
    public Guid MemberId { get; }
    public LendingDate LendingDate { get; }
    public ReturnDate ReturnDate { get; }

    public LendingCreated(
        Guid bookId,
        Guid libraryId,
        Guid memberId,
        LendingDate lendingDate,
        ReturnDate returnDate)
        : base(new { bookId, libraryId, memberId, lendingDate, returnDate })
    {
        if (bookId == Guid.Empty)
            throw new ArgumentException("BookId cannot be empty.", nameof(bookId));
        if (libraryId == Guid.Empty)
            throw new ArgumentException("LibraryId cannot be empty.", nameof(libraryId));
        if (memberId == Guid.Empty)
            throw new ArgumentException("MemberId cannot be empty.", nameof(memberId));
        if (lendingDate == null)
            throw new ArgumentNullException(nameof(lendingDate));
        if (returnDate == null)
            throw new ArgumentNullException(nameof(returnDate));

        BookId = bookId;
        LibraryId = libraryId;
        MemberId = memberId;
        LendingDate = lendingDate;
        ReturnDate = returnDate;
    }
}
