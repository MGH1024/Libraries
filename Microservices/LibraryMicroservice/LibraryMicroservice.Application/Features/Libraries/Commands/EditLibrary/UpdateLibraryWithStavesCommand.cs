using Application.Features.Libraries.Extensions;
using Application.Features.Libraries.Rules;
using Domain.Entities.Libraries;
using Domain.Entities.Libraries.Constant;
using MGH.Core.Application.Buses.Commands;
using MGH.Core.Persistence.UnitOfWork;

namespace Application.Features.Libraries.Commands.EditLibrary;

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

public class EditLibraryWithStavesCommandHandler : ICommandHandler<UpdateLibraryWithStavesCommand, Guid>
{
    private readonly ILibraryRepository _libraryRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly LibraryBusinessRules _libraryBusinessRules;

    public EditLibraryWithStavesCommandHandler(ILibraryRepository libraryRepository, IUnitOfWork unitOfWork,
        LibraryBusinessRules libraryBusinessRules)
    {
        _libraryRepository = libraryRepository;
        _unitOfWork = unitOfWork;
        _libraryBusinessRules = libraryBusinessRules;
    }

    public async Task<Guid> Handle(UpdateLibraryWithStavesCommand request, CancellationToken cancellationToken)
    {
        var library = 
            await _libraryRepository.GetAsync(a => a.Id == request.LibraryId, cancellationToken: cancellationToken);
        await _libraryBusinessRules.LibraryShouldBeExistsWhenSelected(library);
        
        if (request.Code != library.LibraryCode)
            await _libraryBusinessRules.LibraryCodeMustBeUnique(request.Code);
        
        library.EditLibrary(request.Name, request.Code, request.Location, request.District,
            request.RegistrationDate,request.StavesDto.ToStaffList());
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return library.Id;
    }
}