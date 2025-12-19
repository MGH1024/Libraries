using MGH.Core.Domain.Events;

namespace Library.Domain.Libraries.Events;

public sealed class StaffDeleted : DomainEvent
{
    public Guid LibraryId { get; }
    public string NationalCode { get; }

    public StaffDeleted(
        Guid libraryId,
        string nationalCode)
        : base(new
        {
            libraryId,
            nationalCode
        })
    {
        if (libraryId == Guid.Empty)
            throw new ArgumentException("LibraryId cannot be empty.", nameof(libraryId));

        if (string.IsNullOrWhiteSpace(nationalCode))
            throw new ArgumentException("NationalCode cannot be null or empty.", nameof(nationalCode));

        LibraryId = libraryId;
        NationalCode = nationalCode;
    }
}
