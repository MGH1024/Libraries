using MGH.Core.Domain.Events;
using Library.Domain.Libraries.Constant;
using MGH.Core.Application.Buses.Commands;
using System.Text.Json.Serialization;

namespace Library.Domain.Libraries.Events;

public sealed class LibraryCreatedDomainEvent : DomainEvent, ICommand
{
    // Properties must have setters for deserialization
    public string LibraryName { get; set; }
    public string LibraryCode { get; set; }
    public string LibraryLocation { get; set; }
    public DistrictEnum LibraryDistrict { get; set; }
    public DateTime LibraryRegistrationDate { get; set; }


    // Constructor for creating events in code
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
