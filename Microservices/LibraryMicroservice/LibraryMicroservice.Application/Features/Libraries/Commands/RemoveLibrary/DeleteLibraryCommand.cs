using Application.Features.Libraries.Rules;
using Domain.Entities.Libraries;
using MediatR;
using MGH.Core.Application.Buses.Commands;
using MGH.Core.Persistence.UnitOfWork;

namespace Application.Features.Libraries.Commands.RemoveLibrary;

public class DeleteLibraryCommand : ICommand<Unit>
{
    public Guid LibraryId { get; set; }
}

public class RemoveLibraryCommandHandler : ICommandHandler<DeleteLibraryCommand, Unit>
{
    private readonly ILibraryRepository _libraryRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly LibraryBusinessRules _libraryBusinessRules;

    public RemoveLibraryCommandHandler(ILibraryRepository libraryRepository, IUnitOfWork unitOfWork,
        LibraryBusinessRules libraryBusinessRules)
    {
        _libraryRepository = libraryRepository;
        _unitOfWork = unitOfWork;
        _libraryBusinessRules = libraryBusinessRules;
    }

    public async Task<Unit> Handle(DeleteLibraryCommand request, CancellationToken cancellationToken)
    {
        var library =
            await _libraryRepository.GetAsync(a => a.Id == request.LibraryId, cancellationToken: cancellationToken);
        await _libraryBusinessRules.LibraryShouldBeExistsWhenSelected(library);

        await library.RemoveLibrary(library);
        await _libraryRepository.DeleteAsync(library, true);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}