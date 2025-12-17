using MediatR;
using Library.Domain;
using MGH.Core.Application.Buses;
using Library.Domain.Libraries.Exceptions;

namespace Library.Application.Features.PublicLibraries.Commands.Remove;

public class RemoveCommandHandler(IUow uow)
    : ICommandHandler<RemoveCommand, Unit>
{
    public async Task<Unit> Handle(
        RemoveCommand request,
        CancellationToken cancellationToken)
    {
        var library = await uow.Library.GetAsync(
            request.Id,
            cancellationToken)
                ?? throw new LibraryNotFoundException();

        library.Remove();
        await uow.Library.DeleteAsync(library);
        await uow.CompleteAsync(cancellationToken);
        return Unit.Value;
    }
}