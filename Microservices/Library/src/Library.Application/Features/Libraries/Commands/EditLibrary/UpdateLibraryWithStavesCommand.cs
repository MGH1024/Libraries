using Application.Features.Libraries.Constants;
using Domain;
using MGH.Core.Domain.Buses.Commands;
using Domain.Entities.Libraries.Constant;
using Application.Features.Libraries.Rules;
using Application.Features.Libraries.Extensions;
using MGH.Core.Application.Pipelines.Authorization;

namespace Application.Features.Libraries.Commands.EditLibrary;

[Roles(LibraryOperationClaims.UpdateLibraryWithStaves)]
public class UpdateLibraryWithStavesCommand : ICommand<Guid>
{
    public Guid LibraryId { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
    public string Location { get; set; }
    public District District { get; set; }
    public DateTime RegistrationDate { get; set; }
    public List<StaffDto> StavesDto { get; set; }
}

public class EditLibraryWithStavesCommandHandler(IUow uow,
    LibraryBusinessRules libraryBusinessRules)
    : ICommandHandler<UpdateLibraryWithStavesCommand, Guid>
{
    public async Task<Guid> Handle(UpdateLibraryWithStavesCommand request, CancellationToken cancellationToken)
    {
        var library = await uow.Library.GetAsync(request.ToGetBaseLibraryModel(cancellationToken));
        await libraryBusinessRules.LibraryShouldBeExistsWhenSelected(library);

        if (request.Code != library.Code)
            await libraryBusinessRules.LibraryCodeMustBeUnique(request.Code);

        library.EditLibrary(request.Name, request.Code, request.Location, request.District,
            request.RegistrationDate, request.StavesDto.ToStaffList());
        await uow.CompleteAsync(cancellationToken);
        return library.Id;
    }
}