using Application.Features.Libraries.Rules;
using Domain.Entities.Libraries;
using Domain.Entities.Libraries.Constant;
using Domain.Entities.Libraries.Factories;
using MGH.Core.Application.Buses.Commands;
using MGH.Core.Persistence.UnitOfWork;

namespace Application.Features.Libraries.Commands.CreateLibrary;

public class CreateLibraryCommand : ICommand<Guid>
{
    public string Name { get; set; }
    public string Code { get; set; }
    public string Location { get; set; }
    public District District { get; set; }
    public DateTime RegistrationDate { get; set; }
}

public class CreateLibraryCommandHandler : ICommandHandler<CreateLibraryCommand, Guid>
{
    private readonly ILibraryRepository _libraryRepository;
    private readonly ILibraryFactory _libraryFactory;
    private readonly IUnitOfWork _unitOfWork;
    private readonly LibraryBusinessRules _libraryBusinessRules;

    public CreateLibraryCommandHandler(ILibraryRepository libraryRepository, ILibraryFactory libraryFactory,
        IUnitOfWork unitOfWork, LibraryBusinessRules libraryBusinessRules)
    {
        _libraryRepository = libraryRepository;
        _libraryFactory = libraryFactory;
        _unitOfWork = unitOfWork;
        _libraryBusinessRules = libraryBusinessRules;
    }

    public async Task<Guid> Handle(CreateLibraryCommand command, CancellationToken cancellationToken)
    {
        await _libraryBusinessRules.LibraryCodeMustBeUnique(command.Code);

        var library = _libraryFactory.Create(command.Name, command.Code, command.Location,
            command.RegistrationDate, command.District);
        await _libraryRepository.AddAsync(library, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return library.Id;
    }
}