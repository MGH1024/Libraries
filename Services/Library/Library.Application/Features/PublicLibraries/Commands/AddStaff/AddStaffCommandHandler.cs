using MediatR;
using Library.Domain;
using MGH.Core.Application.Buses;
using Library.Domain.Libraries.Exceptions;
using Library.Domain.Libraries.ValueObjects;

namespace Library.Application.Features.PublicLibraries.Commands.AddStaff;

public class AddStaffCommandHandler(IUow uow)
    : ICommandHandler<AddStaffCommand, Unit>
{
    public async Task<Unit> Handle(AddStaffCommand request, CancellationToken cancellationToken)
    {
        var library = await uow.Library.GetAsync(request.LibraryId)
            ?? throw new LibraryNotFoundException();
        library.AddLibraryStaff(new Staff(request.Name, request.Position, request.NationalCode));
        await uow.CompleteAsync(cancellationToken);
        return Unit.Value;
    }
}