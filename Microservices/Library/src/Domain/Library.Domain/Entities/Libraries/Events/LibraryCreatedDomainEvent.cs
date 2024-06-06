using MGH.Core.Domain.Aggregate;
using MGH.Core.Domain.Buses.Commands;

namespace Domain.Entities.Libraries.Events;

public record LibraryCreatedDomainEvent(Guid Id, string LibraryName, string LibraryCode, 
    string LibraryLocation, int LibraryDistrict, 
    DateTime LibraryRegistrationDate) : DomainEvent(Id),ICommand;