using MGH.Core.Domain.Events;
using Library.Domain.Libraries.Constant;

namespace Library.Domain.Libraries.Events;

public sealed class LibraryAdded : DomainEvent
{
    public string LibraryName { get; }
    public string LibraryCode { get; }
    public string LibraryLocation { get; }
    public DistrictEnum LibraryDistrict { get; }
    public DateTime LibraryRegistrationDate { get; }

    public LibraryAdded(
        string libraryName,
        string libraryCode,
        string libraryLocation,
        DistrictEnum libraryDistrict,
        DateTime libraryRegistrationDate)
        : base(new
        {
            libraryName,
            libraryCode,
            libraryLocation,
            libraryDistrict,
            libraryRegistrationDate
        })
    {
        if (string.IsNullOrWhiteSpace(libraryName))
            throw new ArgumentException("LibraryName cannot be null or empty.", nameof(libraryName));
        if (string.IsNullOrWhiteSpace(libraryCode))
            throw new ArgumentException("LibraryCode cannot be null or empty.", nameof(libraryCode));
        if (string.IsNullOrWhiteSpace(libraryLocation))
            throw new ArgumentException("LibraryLocation cannot be null or empty.", nameof(libraryLocation));

        LibraryName = libraryName;
        LibraryCode = libraryCode;
        LibraryLocation = libraryLocation;
        LibraryDistrict = libraryDistrict;
        LibraryRegistrationDate = libraryRegistrationDate;
    }
}
