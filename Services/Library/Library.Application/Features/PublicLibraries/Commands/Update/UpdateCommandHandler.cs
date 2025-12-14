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

        if (request.Code != library.Code)
        {
            var libraryByCode = await uow.Library.GetByCodeAsync(request.Code);
            if (libraryByCode is not null)
                throw new LibraryException("library code must be unique");
        }

        library.EditLibrary(request.Name, request.Code, request.Location, request.DistrictEnum, request.RegistrationDate);
        await uow.CompleteAsync(cancellationToken);
        return library.Id;
    }
}