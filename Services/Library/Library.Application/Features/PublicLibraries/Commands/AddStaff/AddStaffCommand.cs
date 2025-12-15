using MediatR;
using MGH.Core.Application.Buses;
using MGH.Core.Application.Pipelines.Authorization;
using Library.Application.Features.PublicLibraries.Constants;

namespace Library.Application.Features.PublicLibraries.Commands.AddStaff;

[Roles(PublicLibraryOperationClaims.AddStaff)]
public class AddStaffCommand : ICommand<Unit>
{
    public Guid LibraryId { get; set; }
    public required string Name { get; set; }
    public required string Position { get; set; }
    public required string NationalCode { get; set; }
}
