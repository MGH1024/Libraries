using Library.Domain;
using MGH.Core.Application.Buses;
using Library.Domain.Libraries.Exceptions;
using Library.Application.Features.PublicLibraries.Profiles;

namespace Library.Application.Features.PublicLibraries.Commands.Update;

public class UpdateLibraryWithStavesCommandHandler(IUow uow)
    : ICommandHandler<UpdateLibraryWithStavesCommand, Guid>
{
    public async Task<Guid> Handle(
        UpdateLibraryWithStavesCommand request,
        CancellationToken cancellationToken)
    {
        var library = await uow.Library.GetAsync(request.LibraryId)
            ?? throw new LibraryNotFoundException();

        if (request.Code != library.Code)
        {
            var libraryByCode = await uow.Library.GetByCodeAsync(request.Code);
            if (libraryByCode is not null)
                throw new LibraryException("library code must be unique");
        }

        library.EditLibrary(
            request.Name,
            request.Code,
            request.Location,
            request.DistrictEnum,
            request.RegistrationDate, 
            request.StavesDto.ToStaffList());
        await uow.CompleteAsync(cancellationToken);
        return library.Id;
    }
}