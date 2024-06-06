using MediatR;
using Domain.Entities.Libraries;
using Application.Features.Libraries.Rules;
using MGH.Core.Domain.Buses.Commands;
using MGH.Core.Infrastructure.Persistence.UnitOfWork;

namespace Application.Features.Libraries.Commands.RemoveLibrary;

public class DeleteLibraryCommand : ICommand<Unit>
{
    public Guid LibraryId { get; set; }
}

public class RemoveLibraryCommandHandler(
    ILibraryRepository libraryRepository,
    IUnitOfWork unitOfWork,
    LibraryBusinessRules libraryBusinessRules)
    : ICommandHandler<DeleteLibraryCommand, Unit>
{
    public async Task<Unit> Handle(DeleteLibraryCommand request, CancellationToken cancellationToken)
    {
        var library =
            await libraryRepository.GetAsync(a => a.Id == request.LibraryId, cancellationToken: cancellationToken);
        await libraryBusinessRules.LibraryShouldBeExistsWhenSelected(library);

        await library.RemoveLibrary(library);
        await libraryRepository.DeleteAsync(library, true);
        await unitOfWork.CompleteAsync(cancellationToken);
        return Unit.Value;
    }
}