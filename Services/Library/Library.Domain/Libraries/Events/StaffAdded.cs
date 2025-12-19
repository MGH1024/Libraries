using MGH.Core.Domain.Events;

namespace Library.Domain.Libraries.Events;

public sealed class StaffAdded : DomainEvent
{
    public Guid LibraryId { get; }
    public string Name { get; }
    public string Position { get; }
    public string NationalCode { get; }

    public StaffAdded(
        Guid libraryId,
        string name,
        string position,
        string nationalCode)
        : base(new
        {
            libraryId,
            name,
            position,
            nationalCode
        })
    {
        if (libraryId == Guid.Empty)
            throw new ArgumentException("LibraryId cannot be empty.", nameof(libraryId));

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be null or empty.", nameof(name));

        if (string.IsNullOrWhiteSpace(position))
            throw new ArgumentException("Position cannot be null or empty.", nameof(position));

        if (string.IsNullOrWhiteSpace(nationalCode))
            throw new ArgumentException("NationalCode cannot be null or empty.", nameof(nationalCode));

        LibraryId = libraryId;
        Name = name;
        Position = position;
        NationalCode = nationalCode;
    }
}
