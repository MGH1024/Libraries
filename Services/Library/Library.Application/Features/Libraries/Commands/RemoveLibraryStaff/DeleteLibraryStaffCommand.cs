using MediatR;
using Library.Domain;
using MGH.Core.Domain.Buses.Commands;
using MGH.Core.Application.Pipelines.Authorization;
using Library.Application.Features.Libraries.Profiles;
using Library.Application.Features.Libraries.Constants;

namespace Library.Application.Features.Libraries.Commands.RemoveLibraryStaff;

[Roles(LibraryOperationClaims.DeleteLibraryStaves)]
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
        var library = await uow.Library.GetAsync(request.ToGetBaseLibraryModel());
        library.RemoveLibraryStaff(request.NationalCode);
        await uow.CompleteAsync(cancellationToken);
        return Unit.Value;
    }
}