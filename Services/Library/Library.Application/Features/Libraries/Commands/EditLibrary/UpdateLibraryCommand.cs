using Library.Domain;
using MGH.Core.Domain.Buses.Commands;
using Library.Domain.Libraries.Constant;
using Library.Application.Features.Libraries.Rules;
using MGH.Core.Application.Pipelines.Authorization;
using Library.Application.Features.Libraries.Profiles;
using Library.Application.Features.Libraries.Constants;

namespace Library.Application.Features.Libraries.Commands.EditLibrary;

[Roles(LibraryOperationClaims.UpdateLibraries)]
public class UpdateLibraryCommand : ICommand<Guid>
{
    public Guid LibraryId { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
    public string Location { get; set; }
    public DistrictEnum DistrictEnum { get; set; }
    public DateTime RegistrationDate { get; set; }
}

public class EditLibraryCommandHandler(
    IUow uow,
    ILibraryBusinessRules libraryBusinessRules)
    : ICommandHandler<UpdateLibraryCommand, Guid>
{
    public async Task<Guid> Handle(UpdateLibraryCommand request, CancellationToken cancellationToken)
    {
        var library = await uow.Library.GetAsync(request.ToGetBaseLibraryModel());
        await libraryBusinessRules.LibraryShouldBeExistsWhenSelected(library);

        if (request.Code != library.Code)
            await libraryBusinessRules.LibraryCodeMustBeUnique(request.Code);

        library.EditLibrary(request.Name, request.Code, request.Location, request.DistrictEnum, request.RegistrationDate);
        await uow.CompleteAsync(cancellationToken);
        return library.Id;
    }
}