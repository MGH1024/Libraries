using Library.Domain.Lendings.Events;
using Library.Domain.Lendings.ValueObjects;
using MGH.Core.Domain.BaseEntity;

namespace Library.Domain.Lendings;

public class Lending : AggregateRoot<Guid>
{
    public Guid BookId { get;private set;  }
    public Guid LibraryId { get; private set; }
    public Guid MemberId { get; private set; }
    public LendingDate LendingDate { get;private set;  }
    private protected ReturnDate ReturnDate { get; private set; }

    private Lending()
    {
        
    }

    public Lending(Guid bookId, Guid libraryId, Guid memberId, LendingDate lendingDate,ReturnDate returnDate)
    {
        BookId = bookId;
        MemberId = memberId;
        LibraryId = libraryId;
        LendingDate = lendingDate;
        ReturnDate = returnDate;
        AddEvent(new LendingCreatedDomainEvent(bookId, libraryId, memberId, lendingDate,returnDate));
    }
    
}