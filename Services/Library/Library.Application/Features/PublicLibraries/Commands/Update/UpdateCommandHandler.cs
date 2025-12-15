using Library.Domain;
using MGH.Core.Application.Buses;
using Library.Domain.Libraries.Exceptions;

namespace Library.Application.Features.PublicLibraries.Commands.Update;

public class UpdateCommandHandler(IUow uow)
    : ICommandHandler<UpdateCommand, Guid>
{
    public async Task<Guid> Handle(
        UpdateCommand request,
        CancellationToken cancellationToken)
    {
        var library = await uow.Library.GetAsync(request.LibraryId);
        if (library is null)
            throw new LibraryNotFoundException();

        library.UpdateLibrary(request.Name, request.Location, request.District, request.RegistrationTime);
        await uow.CompleteAsync(cancellationToken);
        return library.Id;
    }
}