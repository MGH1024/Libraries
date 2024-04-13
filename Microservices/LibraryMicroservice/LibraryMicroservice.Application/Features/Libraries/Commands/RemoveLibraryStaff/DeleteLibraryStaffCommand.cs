using MediatR;
using Domain.Entities.Libraries;
using Microsoft.EntityFrameworkCore;
using MGH.Core.Persistence.UnitOfWork;
using MGH.Core.Application.Buses.Commands;

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
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}