using MediatR;
using Library.Domain;
using MGH.Core.Domain.Buses.Commands;
using Library.Application.Features.Libraries.Rules;
using MGH.Core.Application.Pipelines.Authorization;
using Library.Application.Features.Libraries.Profiles;
using Library.Application.Features.Libraries.Constants;

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
        var library = await uow.Library.GetAsync(request.ToGetBaseLibraryModel());
        await libraryBusinessRules.LibraryShouldBeExistsWhenSelected(library);

        await Domain.Libraries.Library.RemoveLibrary(library);
        await uow.Library.DeleteAsync(library);
        await uow.CompleteAsync(cancellationToken);
        return Unit.Value;
    }
}