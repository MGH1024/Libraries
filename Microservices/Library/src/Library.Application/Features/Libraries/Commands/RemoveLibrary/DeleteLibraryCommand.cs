using Library.Application.Features.Libraries.Constants;
using Library.Application.Features.Libraries.Profiles;
using Library.Application.Features.Libraries.Rules;
using Library.Domain;
using MediatR;
using MGH.Core.Application.Pipelines.Authorization;
using MGH.Core.Domain.Buses.Commands;

namespace Library.Application.Features.Libraries.Commands.RemoveLibrary;

[Roles(LibraryOperationClaims.DeleteLibraries)]
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
        var library = await uow.Library.GetAsync(request.ToGetBaseLibraryModel(cancellationToken));
        await libraryBusinessRules.LibraryShouldBeExistsWhenSelected(library);

        await Library.Domain.Entities.Libraries.Library.RemoveLibrary(library);
        await uow.Library.DeleteAsync(library, true);
        await uow.CompleteAsync(cancellationToken);
        return Unit.Value;
    }
}