using Library.Application.Features.PublicLibraries.Profiles;
using Library.Domain;
using Library.Domain.Libraries.Exceptions;
using Library.Domain.Libraries.ValueObjects;
using MGH.Core.Application.Buses;

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
            ToStaffList(request.StavesDto));
        await uow.CompleteAsync(cancellationToken);
        return library.Id;
    }

    public IEnumerable<Staff> ToStaffList(List<StaffDto> staffDtos)
    {
        return staffDtos.Select(ToStaff);
    }

    private Staff ToStaff(StaffDto staffDto)
    {
        return new Staff(
            staffDto.Name,
            staffDto.Position,
            staffDto.NationalCode);
    }

}