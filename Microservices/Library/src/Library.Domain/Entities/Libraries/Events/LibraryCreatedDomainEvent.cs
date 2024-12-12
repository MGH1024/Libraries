using MGH.Core.Domain.Buses.Commands;
using Domain.Entities.Libraries.Constant;
using MGH.Core.Domain.BaseEntity.Abstract.Events;
using MGH.Core.Infrastructure.MessageBroker.RabbitMq.Attributes;

namespace Domain.Entities.Libraries.Events;

[BaseMessage(
    QueueItemProperty.CommonRoutingKey,
    QueueItemProperty.CommonExchangeType,
    QueueItemProperty.CommonExchangeName,
    QueueItemProperty.CommonQueueName
)]
public class LibraryCreatedDomainEvent(string libraryName, string libraryCode, string libraryLocation, DistrictEnum libraryDistrict, DateTime libraryRegistrationDate)
    : DomainEvent, ICommand
{
    public string LibraryName { get; set; } = libraryName;
    public string LibraryCode { get; set; } = libraryCode;
    public string LibraryLocation { get; set; } = libraryLocation;
    public DistrictEnum LibraryDistrict { get; set; } = libraryDistrict;
    public DateTime LibraryRegistrationDate { get; set; } = libraryRegistrationDate;
}