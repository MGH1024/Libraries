using Library.Domain.Libraries.Constant;
using MGH.Core.Domain.Buses.Commands;
using MGH.Core.Domain.Events;
using MGH.Core.Infrastructure.EventBus.RabbitMq;
using MGH.Core.Infrastructure.EventBus.RabbitMq.Attributes;

namespace Library.Domain.Libraries.Events;

[EventRouting(QueueItemProperty.CommonRoutingKey, QueueItemProperty.CommonExchangeType)]
public class LibraryCreatedDomainEvent(string libraryName, string libraryCode, string libraryLocation, DistrictEnum libraryDistrict, DateTime libraryRegistrationDate)
    : DomainEvent, ICommand
{
    public string LibraryName { get; set; } = libraryName;
    public string LibraryCode { get; set; } = libraryCode;
    public string LibraryLocation { get; set; } = libraryLocation;
    public DistrictEnum LibraryDistrict { get; set; } = libraryDistrict;
    public DateTime LibraryRegistrationDate { get; set; } = libraryRegistrationDate;
}