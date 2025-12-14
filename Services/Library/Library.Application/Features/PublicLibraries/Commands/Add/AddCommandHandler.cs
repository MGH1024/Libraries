using Library.Domain;
using MGH.Core.Application.Buses;
using MGH.Core.Infrastructure.EventBus;
using Library.Domain.Libraries.Factories;
using Library.Application.Features.PublicLibraries.Rules;
using Library.Application.Features.PublicLibraries.Profiles;

namespace Library.Application.Features.PublicLibraries.Commands.Add;

public class AddCommandHandler(
    IUow uow,
    IEventBus eventBus,
    IPublicLibraryFactory libraryFactory,
    ILibraryBusinessRules libraryBusinessRules) : ICommandHandler<AddCommand, Guid>
{
    public async Task<Guid> Handle(AddCommand command, CancellationToken cancellationToken)
    {
        await libraryBusinessRules.LibraryCodeMustBeUnique(command.Code);

        var library = libraryFactory.Create(command.Name, command.Code, command.Location,
            command.RegistrationTime, command.District);

        await uow.Library.AddAsync(library);

        await eventBus.PublishAsync(library.ToLibraryCreatedDomainEvent(),
            PublishMode.Direct,
            cancellationToken);
        await uow.CompleteAsync(cancellationToken);
        return library.Id;
    }
}