using MediatR;
using MGH.Core.Application.Buses;
using MGH.Core.Application.Pipelines.Authorization;
using Library.Application.Features.PublicLibraries.Constants;

namespace Library.Application.Features.PublicLibraries.Commands.RemoveStaff;

[Roles(PublicLibraryOperationClaims.RemoveStaves)]
public class RemoveStaffCommand : ICommand<Unit>
{
    public Guid LibraryId { get; set; }
    public required string NationalCode { get; set; }
}
