using MGH.Core.Domain.Events;
using Library.Domain.Libraries.Constant;
using MGH.Core.Application.Buses.Commands;

namespace Library.Domain.Libraries.Events;

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
        })
    {
    }
}
