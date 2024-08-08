using Application.Features.Libraries.Extensions;
using MediatR;
using Domain.Entities.Libraries;
using Application.Features.Libraries.Rules;
using MGH.Core.Domain.Buses.Commands;
using MGH.Core.Infrastructure.Persistence.Persistence.Base;
using MGH.Core.Infrastructure.Persistence.Persistence.Models.Filters;

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
        var library = await libraryRepository.GetAsync(request.ToGetBaseLibraryModel(cancellationToken));
        await libraryBusinessRules.LibraryShouldBeExistsWhenSelected(library);

        await library.RemoveLibrary(library);
        await libraryRepository.DeleteAsync(library, true);
        await unitOfWork.CompleteAsync(cancellationToken);
        return Unit.Value;
    }
}