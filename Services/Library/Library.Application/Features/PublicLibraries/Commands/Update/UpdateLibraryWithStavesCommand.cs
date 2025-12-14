using Library.Domain;
using MGH.Core.Application.Buses;
using Library.Domain.Libraries.Constant;
using MGH.Core.Application.Pipelines.Authorization;
using Library.Application.Features.PublicLibraries.Rules;
using Library.Application.Features.PublicLibraries.Profiles;
using Library.Application.Features.PublicLibraries.Constants;

namespace Library.Application.Features.PublicLibraries.Commands.Update;

[Roles(PublicLibraryOperationClaims.UpdateWithStaves)]
public class UpdateLibraryWithStavesCommand : ICommand<Guid>
{
    public Guid LibraryId { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
    public string Location { get; set; }
    public DistrictEnum DistrictEnum { get; set; }
    public DateTime RegistrationDate { get; set; }
    public List<StaffDto> StavesDto { get; set; }
}

public class EditLibraryWithStavesCommandHandler(IUow uow,
    ILibraryBusinessRules libraryBusinessRules)
    : ICommandHandler<UpdateLibraryWithStavesCommand, Guid>
{
    public async Task<Guid> Handle(UpdateLibraryWithStavesCommand request, CancellationToken cancellationToken)
    {
        var library = await uow.Library.GetAsync(request.ToGetBaseLibraryModel());
        await libraryBusinessRules.LibraryShouldBeExistsWhenSelected(library);

        if (request.Code != library.Code)
            await libraryBusinessRules.LibraryCodeMustBeUnique(request.Code);

        library.EditLibrary(request.Name, request.Code, request.Location, request.DistrictEnum,
            request.RegistrationDate, request.StavesDto.ToStaffList());
        await uow.CompleteAsync(cancellationToken);
        return library.Id;
    }
}