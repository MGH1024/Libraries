using MediatR;
using Library.Domain;
using MGH.Core.Application.Buses;
using Library.Domain.Libraries.Exceptions;
using Library.Domain.Libraries.ValueObjects;

namespace Library.Application.Features.PublicLibraries.Commands.AddStaff;

public class AddStaffCommandHandler(IUow uow)
    : ICommandHandler<AddStaffCommand, Unit>
{
    public async Task<Unit> Handle(
        AddStaffCommand request,
        CancellationToken cancellationToken)
    {
        var library = await uow.Library.GetAsync(request.LibraryId)
            ?? throw new LibraryNotFoundException();

        var staff = ToStaff(request);
        library.AddLibraryStaff(staff);

        await uow.CompleteAsync(cancellationToken);
        return Unit.Value;
    }

    private Staff ToStaff(AddStaffCommand command)
    {
        return new Staff(
            command.Name,
            command.Position,
            command.NationalCode);
    }
}