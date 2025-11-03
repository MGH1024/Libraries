using MGH.Core.Domain.Events;
using Library.Domain.Libraries.Constant;
using MGH.Core.Infrastructure.EventBus.RabbitMq.Attributes;
using MGH.Core.Domain.Buses.Commands;

namespace Library.Domain.Libraries.Events;

[EventRouting(QueueItemProperty.CommonRoutingKey, QueueItemProperty.CommonExchangeType)]
public sealed class LibraryCreatedDomainEvent : DomainEvent,ICommand
{
    public string LibraryName { get; }
    public string LibraryCode { get; }
    public string LibraryLocation { get; }
    public DistrictEnum LibraryDistrict { get; }
    public DateTime LibraryRegistrationDate { get; }

    public LibraryCreatedDomainEvent(
        string libraryName,
        string libraryCode,
        string libraryLocation,
        DistrictEnum libraryDistrict,
        DateTime libraryRegistrationDate)
        : base(new { libraryName, libraryCode, libraryLocation, libraryDistrict, libraryRegistrationDate })
    {
        LibraryName = libraryName;
        LibraryCode = libraryCode;
        LibraryLocation = libraryLocation;
        LibraryDistrict = libraryDistrict;
        LibraryRegistrationDate = libraryRegistrationDate;
    }
}
