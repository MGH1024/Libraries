using System;
using MGH.Core.Domain.Events;
using Library.Domain.Libraries.Constant;

namespace Library.Domain.Libraries.Events;

public sealed class LibraryUpdated : DomainEvent
{
    public Guid LibraryId { get; }
    public string LibraryName { get; }
    public string LibraryLocation { get; }
    public DistrictEnum LibraryDistrict { get; }
    public DateTime LibraryRegistrationDate { get; }

    public LibraryUpdated(
        Guid libraryId,
        string libraryName,
        string libraryLocation,
        DistrictEnum libraryDistrict,
        DateTime libraryRegistrationDate)
        : base(new
        {
            libraryId,
            libraryName,
            libraryLocation,
            libraryDistrict,
            libraryRegistrationDate
        })
    {
        if (libraryId == Guid.Empty)
            throw new ArgumentException("LibraryId cannot be empty.", nameof(libraryId));

        if (string.IsNullOrWhiteSpace(libraryName))
            throw new ArgumentException("LibraryName cannot be null or empty.", nameof(libraryName));

        if (string.IsNullOrWhiteSpace(libraryLocation))
            throw new ArgumentException("LibraryLocation cannot be null or empty.", nameof(libraryLocation));

        LibraryId = libraryId;
        LibraryName = libraryName;
        LibraryLocation = libraryLocation;
        LibraryDistrict = libraryDistrict;
        LibraryRegistrationDate = libraryRegistrationDate;
    }
}
