using MGH.Core.Domain.Events;
using MGH.Core.Application.Buses;

namespace Library.Domain.Libraries.Events;

public sealed class StaffDeletedDomainEvent : DomainEvent, ICommand
{
    public Guid LibraryId { get; set; }
    public string NationalCode { get; set; }

    public StaffDeletedDomainEvent(
        Guid libraryId,
        string nationalCode)
        : base(new
        {
            libraryId,
            nationalCode
        })
    {
        LibraryId = libraryId;
        NationalCode = nationalCode;
    }
}