using MediatR;
using MGH.Core.Application.Buses.Commands;
using MGH.Core.Application.Pipelines.Authorization;
using Library.Application.Features.PublicLibraries.Constants;

namespace Library.Application.Features.PublicLibraries.Commands.Remove;

[Roles(PublicLibraryOperationClaims.Remove)]
public class RemoveCommand : ICommand<Unit>
{
    public Guid LibraryId { get; set; }
}
