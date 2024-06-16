using Application.Features.Libraries.Rules;
using Domain.Entities.Libraries;
using Domain.Entities.Libraries.Constant;
using MGH.Core.Domain.Buses.Commands;
using MGH.Core.Infrastructure.Persistence.Persistence.Base;

namespace Application.Features.Libraries.Commands.EditLibrary;

public class UpdateLibraryCommand : ICommand<Guid>
{
    public Guid LibraryId { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
    public string Location { get; set; }
    public District District { get; set; }
    public DateTime RegistrationDate { get; set; }
}

public class EditLibraryCommandHandler(
    ILibraryRepository libraryRepository,
    IUnitOfWork unitOfWork,
    LibraryBusinessRules libraryBusinessRules)
    : ICommandHandler<UpdateLibraryCommand, Guid>
{
    public async Task<Guid> Handle(UpdateLibraryCommand request, CancellationToken cancellationToken)
    {
        var library =
            await libraryRepository.GetAsync(a => a.Id == request.LibraryId, cancellationToken: cancellationToken);
        await libraryBusinessRules.LibraryShouldBeExistsWhenSelected(library);

        if (request.Code != library.LibraryCode)
            await libraryBusinessRules.LibraryCodeMustBeUnique(request.Code);

        library.EditLibrary(request.Name, request.Code, request.Location, request.District, request.RegistrationDate);
        await unitOfWork.CompleteAsync(cancellationToken);
        return library.Id;
    }
}