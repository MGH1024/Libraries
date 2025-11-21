using MGH.Core.Domain.Events;
using Library.Domain.Libraries.Constant;
using MGH.Core.Application.Buses.Commands;
using MGH.Core.Infrastructure.EventBus.RabbitMq.Attributes;

namespace Library.Domain.Libraries.Events;

[EventRouting(QueueItemProperty.LibraryCreatedDomainEventRoutingKey)]
public class LibraryCreatedDomainEvent2(
    string libraryName,
    string libraryCode,
    string libraryLocation,
    DistrictEnum libraryDistrict,
    DateTime libraryRegistrationDate)
    : DomainEvent(new { libraryName, libraryCode, libraryLocation, libraryDistrict, libraryRegistrationDate }), ICommand
{
    public string LibraryName { get; set; } = libraryName;
    public string LibraryCode { get; set; } = libraryCode;
    public string LibraryLocation { get; set; } = libraryLocation;
    public DistrictEnum LibraryDistrict { get; set; } = libraryDistrict;
    public DateTime LibraryRegistrationDate { get; set; } = libraryRegistrationDate;
}
