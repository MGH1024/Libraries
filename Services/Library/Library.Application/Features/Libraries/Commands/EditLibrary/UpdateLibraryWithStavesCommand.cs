using Library.Application.Features.Libraries.Constants;
using Library.Application.Features.Libraries.Profiles;
using Library.Application.Features.Libraries.Rules;
using Library.Domain;
using Library.Domain.Libraries.Constant;
using MGH.Core.Application.Pipelines.Authorization;
using MGH.Core.Domain.Buses.Commands;

namespace Library.Application.Features.Libraries.Commands.EditLibrary;

[Roles(LibraryOperationClaims.UpdateLibraryWithStaves)]
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
        var library = await uow.Library.GetAsync(request.ToGetBaseLibraryModel(cancellationToken));
        await libraryBusinessRules.LibraryShouldBeExistsWhenSelected(library);

        if (request.Code != library.Code)
            await libraryBusinessRules.LibraryCodeMustBeUnique(request.Code);

        library.EditLibrary(request.Name, request.Code, request.Location, request.DistrictEnum,
            request.RegistrationDate, request.StavesDto.ToStaffList());
        await uow.CompleteAsync(cancellationToken);
        return library.Id;
    }
}