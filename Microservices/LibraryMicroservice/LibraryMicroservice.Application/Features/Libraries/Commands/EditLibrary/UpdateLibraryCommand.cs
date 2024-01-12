using Application.Features.Libraries.Rules;
using Domain.Entities.Libraries;
using Domain.Entities.Libraries.Constant;
using MGH.Core.Application.Buses.Commands;
using MGH.Core.Persistence.UnitOfWork;

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

public class EditLibraryCommandHandler : ICommandHandler<UpdateLibraryCommand, Guid>
{
    private readonly ILibraryRepository _libraryRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly LibraryBusinessRules _libraryBusinessRules;

    public EditLibraryCommandHandler(ILibraryRepository libraryRepository, IUnitOfWork unitOfWork,
        LibraryBusinessRules libraryBusinessRules)
    {
        _libraryRepository = libraryRepository;
        _unitOfWork = unitOfWork;
        _libraryBusinessRules = libraryBusinessRules;
    }

    public async Task<Guid> Handle(UpdateLibraryCommand request, CancellationToken cancellationToken)
    {
        var library =
            await _libraryRepository.GetAsync(a => a.Id == request.LibraryId, cancellationToken: cancellationToken);
        await _libraryBusinessRules.LibraryShouldBeExistsWhenSelected(library);

        if (request.Code != library.LibraryCode)
            await _libraryBusinessRules.LibraryCodeMustBeUnique(request.Code);

        library.EditLibrary(request.Name, request.Code, request.Location, request.District, request.RegistrationDate);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return library.Id;
    }
}