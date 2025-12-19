using System;
using MGH.Core.Domain.Events;

namespace Library.Domain.Libraries.Events;

public sealed class LibraryDeleted : DomainEvent
{
    public Guid LibraryId { get; }

    public LibraryDeleted(Guid libraryId)
        : base(new { libraryId })
    {
        if (libraryId == Guid.Empty)
            throw new ArgumentException("LibraryId cannot be empty.", nameof(libraryId));

        LibraryId = libraryId;
    }
}
