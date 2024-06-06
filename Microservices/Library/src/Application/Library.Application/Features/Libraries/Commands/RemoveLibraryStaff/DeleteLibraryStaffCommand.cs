using MediatR;
using Domain.Entities.Libraries;
using Microsoft.EntityFrameworkCore;
using MGH.Core.Domain.Buses.Commands;
using MGH.Core.Infrastructure.Persistence.UnitOfWork;

namespace Application.Features.Libraries.Commands.RemoveLibraryStaff;

public class DeleteLibraryStaffCommand : ICommand<Unit>
{
    public string NationalCode { get; set; }
    public Guid LibraryId { get; set; }
}

public class RemoveLibraryStaffCommandHandler(ILibraryRepository libraryRepository, IUnitOfWork unitOfWork)
    : ICommandHandler<DeleteLibraryStaffCommand, Unit>
{
    public async Task<Unit> Handle(DeleteLibraryStaffCommand request, CancellationToken cancellationToken)
    {
        var library = await libraryRepository
            .GetAsync(a => a.Id == request.LibraryId, a => a.Include(b => b.LibraryStaves),
                cancellationToken: cancellationToken);
        library.RemoveLibraryStaff(request.NationalCode);
        await unitOfWork.CompleteAsync(cancellationToken);
        return Unit.Value;
    }
}