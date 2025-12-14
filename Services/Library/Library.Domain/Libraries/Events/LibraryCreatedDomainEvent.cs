using MGH.Core.Domain.Events;
using MGH.Core.Application.Buses;
using Library.Domain.Libraries.Constant;

namespace Library.Domain.Libraries.Events;

public sealed class LibraryCreatedDomainEvent : DomainEvent, ICommand
{
    public string LibraryName { get; set; }
    public string LibraryCode { get; set; }
    public string LibraryLocation { get; set; }
    public DistrictEnum LibraryDistrict { get; set; }
    public DateTime LibraryRegistrationDate { get; set; }


    public LibraryCreatedDomainEvent(
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
        LibraryName = libraryName;
        LibraryCode = libraryCode;
        LibraryLocation = libraryLocation;
        LibraryDistrict = libraryDistrict;
        LibraryRegistrationDate = libraryRegistrationDate;
    }
}