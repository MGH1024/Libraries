using Application.Features.Libraries.Extensions;
using MediatR;
using Domain.Entities.Libraries;
using Microsoft.EntityFrameworkCore;
using MGH.Core.Domain.Buses.Commands;
using MGH.Core.Infrastructure.Persistence.Persistence.Base;
using MGH.Core.Infrastructure.Persistence.Persistence.Models.Filters;

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
        var library = await libraryRepository.GetAsync(request.ToGetBaseLibraryModel(cancellationToken));
        library.RemoveLibraryStaff(request.NationalCode);
        await unitOfWork.CompleteAsync(cancellationToken);
        return Unit.Value;
    }
}