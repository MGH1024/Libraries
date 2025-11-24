using MediatR;
using Library.Domain;
using Library.Domain.Libraries;
using MGH.Core.Application.Buses.Commands;
using MGH.Core.Application.Pipelines.Authorization;
using Library.Application.Features.PublicLibraries.Profiles;
using Library.Application.Features.PublicLibraries.Constants;
using Library.Application.Features.PublicLibraries.Rules;

namespace Library.Application.Features.PublicLibraries.Commands.RemoveLibrary;

[Roles(PublicLibraryOperationClaims.Delete)]
public class DeleteLibraryCommand : ICommand<Unit>
{
    public Guid LibraryId { get; set; }
}

public class RemoveLibraryCommandHandler(IUow uow,
    ILibraryBusinessRules libraryBusinessRules)
    : ICommandHandler<DeleteLibraryCommand, Unit>
{
    public async Task<Unit> Handle(DeleteLibraryCommand request, CancellationToken cancellationToken)
    {
        var library = await uow.Library.GetAsync(request.ToGetBaseLibraryModel());
        await libraryBusinessRules.LibraryShouldBeExistsWhenSelected(library);

        await PublicLibrary.RemoveLibrary(library);
        await uow.Library.DeleteAsync(library);
        await uow.CompleteAsync(cancellationToken);
        return Unit.Value;
    }
}