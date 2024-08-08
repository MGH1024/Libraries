using Domain;
using MediatR;
using MGH.Core.Domain.Buses.Commands;
using Application.Features.Libraries.Extensions;

namespace Application.Features.Libraries.Commands.RemoveLibraryStaff;

public class DeleteLibraryStaffCommand : ICommand<Unit>
{
    public string NationalCode { get; set; }
    public Guid LibraryId { get; set; }
}

public class RemoveLibraryStaffCommandHandler( IUow uow)
    : ICommandHandler<DeleteLibraryStaffCommand, Unit>
{
    public async Task<Unit> Handle(DeleteLibraryStaffCommand request, CancellationToken cancellationToken)
    {
        var library = await uow.Library.GetAsync(request.ToGetBaseLibraryModel(cancellationToken));
        library.RemoveLibraryStaff(request.NationalCode);
        await uow.CompleteAsync(cancellationToken);
        return Unit.Value;
    }
}