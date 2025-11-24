using MediatR;
using MGH.Core.Application.Buses.Commands;
using MGH.Core.Application.Pipelines.Authorization;
using Library.Application.Features.PublicLibraries.Constants;

namespace Library.Application.Features.PublicLibraries.Commands.RemoveStaff;

[Roles(PublicLibraryOperationClaims.RemoveStaves)]
public class RemoveStaffCommand : ICommand<Unit>
{
    public required string NationalCode { get; set; }
    public Guid LibraryId { get; set; }
}
