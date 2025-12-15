using Library.Domain;
using MGH.Core.Application.Buses;
using MGH.Core.Infrastructure.EventBus;
using Library.Domain.Libraries.Factories;
using Library.Domain.Libraries.Exceptions;

namespace Library.Application.Features.PublicLibraries.Commands.Add;

public class AddCommandHandler(
    IUow uow,
    IEventBus eventBus,
    IPublicLibraryFactory libraryFactory)
    : ICommandHandler<AddCommand, Guid>
{
    public async Task<Guid> Handle(
        AddCommand command,
        CancellationToken cancellationToken)
    {
        var existingLibrary =
            await uow.Library.GetByCodeAsync(command.Code);

        if (existingLibrary is not null)
            throw new LibraryException("library code must be unique");

        var newLibrary = libraryFactory.Create(
            command.Name,
            command.Code,
            command.Location,
            command.RegistrationTime,
            command.District);

        await uow.Library.AddAsync(
            newLibrary,
            cancellationToken: cancellationToken);
        await uow.CompleteAsync(cancellationToken);

        foreach (var domainEvent in newLibrary.DomainEvents)
        {
            await eventBus.PublishAsync(
                domainEvent,
                PublishMode.Outbox,
                cancellationToken);
        }

        newLibrary.ClearDomainEvents();
        return newLibrary.Id;
    }
}
