using MediatR;
using Library.Domain;
using MGH.Core.Application.Buses;
using MGH.Core.Infrastructure.EventBus;
using Library.Domain.Libraries.Exceptions;

namespace Library.Application.Features.PublicLibraries.Commands.Remove;

public class RemoveCommandHandler(
    IUow uow,
    IEventBus eventBus)
    : ICommandHandler<RemoveCommand, Unit>
{
    public async Task<Unit> Handle(
        RemoveCommand request,
        CancellationToken cancellationToken)
    {
        var library = await uow.Library.GetAsync(
            request.LibraryId,
            cancellationToken)
           ?? throw new LibraryNotFoundException();

        library.Remove();
        await uow.Library.DeleteAsync(library);
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