using MGH.Core.Domain.Events;
using MGH.Core.Application.Buses;

namespace Library.Domain.Libraries.Events;

public sealed class StaffAddedDomainEvent : DomainEvent, ICommand
{
    public Guid LibraryId { get; set; }
    public  string Name { get; set; }
    public  string Position { get; set; }
    public  string NationalCode { get; set; }

    public StaffAddedDomainEvent(
        Guid libraryId,
        string name,
        string position,
        string nationalCode)
        : base(new
        {
            libraryId,
            name,
            position,
            nationalCode
        })
    {
        LibraryId = libraryId;
        Name = name;
        Position = position;
        NationalCode = nationalCode;
    }
}
