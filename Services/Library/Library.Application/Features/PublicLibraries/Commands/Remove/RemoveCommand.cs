using MediatR;
using MGH.Core.Application.Buses;
using MGH.Core.Application.Pipelines.Authorization;
using Library.Application.Features.PublicLibraries.Constants;

namespace Library.Application.Features.PublicLibraries.Commands.Remove;

[Roles(PublicLibraryOperationClaims.Remove)]
public class RemoveCommand : ICommand<Unit>
{
    public Guid Id { get; set; }
}
