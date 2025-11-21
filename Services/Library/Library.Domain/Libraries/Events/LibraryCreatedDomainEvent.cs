using MGH.Core.Domain.Events;
using Library.Domain.Libraries.Constant;
using MGH.Core.Application.Buses.Commands;
using MGH.Core.Infrastructure.EventBus.RabbitMq.Attributes;

namespace Library.Domain.Libraries.Events;

[EventRouting(QueueItemProperty.CommonRoutingKey, QueueItemProperty.CommonExchangeType)]
public sealed class LibraryCreatedDomainEvent : DomainEvent, ICommand
{
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
        }, typeof(LibraryCreatedDomainEvent).ToString())
    {
    }
}
