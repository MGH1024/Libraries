using Domain.Entities.Libraries;
using MediatR;
using MGH.Core.Application.Buses.Commands;
using MGH.Core.Persistence.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Libraries.Commands.RemoveLibraryStaff;

public class DeleteLibraryStaffCommand : ICommand<Unit>
{
    public string NationalCode { get; set; }
    public Guid LibraryId { get; set; }
}

public class RemoveLibraryStaffCommandHandler : ICommandHandler<DeleteLibraryStaffCommand, Unit>
{
    private readonly ILibraryRepository _libraryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RemoveLibraryStaffCommandHandler(ILibraryRepository libraryRepository, IUnitOfWork unitOfWork)
    {
        _libraryRepository = libraryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(DeleteLibraryStaffCommand request, CancellationToken cancellationToken)
    {
        var library = await _libraryRepository
            .GetAsync(a => a.Id == request.LibraryId, a => a.Include(b => b.LibraryStaves),
                cancellationToken: cancellationToken);
        library.RemoveLibraryStaff(request.NationalCode);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}