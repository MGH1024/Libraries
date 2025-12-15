using MGH.Core.Domain.Events;
using MGH.Core.Application.Buses;

namespace Library.Domain.Libraries.Events;

public sealed class LibraryDeletedDomainEvent : DomainEvent, ICommand
{
    public Guid LibraryId { get; set; }


    public LibraryDeletedDomainEvent(Guid libraryId)
        : base(new
        {
            libraryId
        })
    {
        LibraryId = libraryId;
    }
}
