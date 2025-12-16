using Library.Domain;
using Library.Domain.Libraries.Exceptions;
using Library.Domain.Libraries.ValueObjects;
using MediatR;
using MGH.Core.Application.Buses;
using MGH.Core.Infrastructure.EventBus;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Library.Application.Features.PublicLibraries.Commands.AddStaff;

public class AddStaffCommandHandler(
    IUow uow,
    IEventBus eventBus)
    : ICommandHandler<AddStaffCommand, Unit>
{
    public async Task<Unit> Handle(
        AddStaffCommand request,
        CancellationToken cancellationToken)
    {
        var library = await uow.Library.GetAsync(request.LibraryId)
            ?? throw new LibraryNotFoundException();

        var staff = new Staff(
            request.Name,
            request.Position,
            request.NationalCode);
        library.AddStaff(staff);
        await uow.CompleteAsync(cancellationToken);

        foreach (var domainEvent in library.DomainEvents)
        {
            await eventBus.PublishAsync(
                domainEvent,
                PublishMode.Outbox,
                cancellationToken);
        }

        library.ClearDomainEvents();
        return Unit.Value;
    }
}