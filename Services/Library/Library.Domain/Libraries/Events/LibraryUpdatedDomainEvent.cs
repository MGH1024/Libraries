using MGH.Core.Domain.Events;
using MGH.Core.Application.Buses;
using Library.Domain.Libraries.Constant;

namespace Library.Domain.Libraries.Events;

public sealed class LibraryUpdatedDomainEvent : DomainEvent, ICommand
{
    public Guid LibraryId { get; set; }
    public string LibraryName { get; set; }
    public string LibraryLocation { get; set; }
    public DistrictEnum LibraryDistrict { get; set; }
    public DateTime LibraryRegistrationDate { get; set; }


    public LibraryUpdatedDomainEvent(
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
        LibraryId = libraryId;
        LibraryName = libraryName;
        LibraryLocation = libraryLocation;
        LibraryDistrict = libraryDistrict;
        LibraryRegistrationDate = libraryRegistrationDate;
    }
}
