using MediatR;
using Library.Domain;
using Library.Domain.Libraries;
using MGH.Core.Application.Buses;
using Library.Application.Features.PublicLibraries.Profiles;
using Library.Application.Features.PublicLibraries.Rules;

namespace Library.Application.Features.PublicLibraries.Commands.Remove;

public class RemoveCommandHandler(IUow uow,
    ILibraryBusinessRules libraryBusinessRules)
    : ICommandHandler<RemoveCommand, Unit>
{
    public async Task<Unit> Handle(RemoveCommand request, CancellationToken cancellationToken)
    {
        var library = await uow.Library.GetAsync(request.ToGetBaseLibraryModel());
        await libraryBusinessRules.LibraryShouldBeExistsWhenSelected(library);

        await PublicLibrary.RemoveLibrary(library);
        await uow.Library.DeleteAsync(library);
        await uow.CompleteAsync(cancellationToken);
        return Unit.Value;
    }
}