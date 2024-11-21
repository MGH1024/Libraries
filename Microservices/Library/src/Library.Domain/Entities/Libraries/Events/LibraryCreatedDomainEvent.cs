using MGH.Core.Domain.BaseEntity;
using MGH.Core.Domain.Buses.Commands;

namespace Domain.Entities.Libraries.Events;

public class LibraryCreatedDomainEvent(
    string libraryName,
    string libraryCode,
    string libraryLocation,
    int libraryDistrict,
    DateTime libraryRegistrationDate)
    : DomainEvent, ICommand
{
    public string LibraryName { get; set; } = libraryName;
    public string LibraryCode { get; set; } = libraryCode;
    public string LibraryLocation { get; set; } = libraryLocation;
    public int LibraryDistrict { get; set; } = libraryDistrict;
    public DateTime LibraryRegistrationDate { get; set; } = libraryRegistrationDate;
}