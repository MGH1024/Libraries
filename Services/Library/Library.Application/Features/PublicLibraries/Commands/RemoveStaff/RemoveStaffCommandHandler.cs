using MediatR;
using Library.Domain;
using MGH.Core.Application.Buses;
using Library.Application.Features.PublicLibraries.Profiles;

namespace Library.Application.Features.PublicLibraries.Commands.RemoveStaff;

public class RemoveStaffCommandHandler( IUow uow)
    : ICommandHandler<RemoveStaffCommand, Unit>
{
    public async Task<Unit> Handle(RemoveStaffCommand request, CancellationToken cancellationToken)
    {
        var library = await uow.Library.GetAsync(request.ToGetBaseLibraryModel());
        library.RemoveLibraryStaff(request.NationalCode);
        await uow.CompleteAsync(cancellationToken);
        return Unit.Value;
    }
}