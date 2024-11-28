using Application.Features.Libraries.Constants;
using Domain;
using MediatR;
using MGH.Core.Domain.Buses.Commands;
using Application.Features.Libraries.Rules;
using Application.Features.Libraries.Extensions;
using MGH.Core.Application.Pipelines.Authorization;


namespace Application.Features.Libraries.Commands.RemoveLibrary;

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

        await library.RemoveLibrary(library);
        await uow.Library.DeleteAsync(library, true);
        await uow.CompleteAsync(cancellationToken);
        return Unit.Value;
    }
}