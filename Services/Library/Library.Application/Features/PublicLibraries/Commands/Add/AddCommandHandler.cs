using Library.Domain;
using Library.Domain.Libraries;
using MGH.Core.Application.Buses;
using Library.Domain.Libraries.Events;
using MGH.Core.Infrastructure.EventBus;
using Library.Domain.Libraries.Factories;
using Library.Domain.Libraries.Exceptions;

namespace Library.Application.Features.PublicLibraries.Commands.Add;

public class AddCommandHandler(
    IUow uow,
    IEventBus eventBus,
    IPublicLibraryFactory libraryFactory) : ICommandHandler<AddCommand, Guid>
{
    public async Task<Guid> Handle(
        AddCommand command,
        CancellationToken cancellationToken)
    {
        var library = await uow.Library.GetByCodeAsync(command.Code);
        if (library is not null)
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

        var @event = ToLibraryCreatedEvent(newLibrary);
        await eventBus.PublishAsync(
            @event,
            PublishMode.Outbox,
            cancellationToken);
        return newLibrary.Id;
    }

    private LibraryCreatedDomainEvent ToLibraryCreatedEvent(PublicLibrary library)
    {
        return new LibraryCreatedDomainEvent(
            library.Name,
            library.Code,
            library.Location,
            library.District,
            library.RegistrationDate);
    }
}