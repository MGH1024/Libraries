using Library.Domain;
using MGH.Core.Infrastructure.EventBus;
using Library.Domain.Libraries.Constant;
using Library.Domain.Libraries.Factories;
using MGH.Core.Application.Buses.Commands;
using Library.Application.Features.Libraries.Rules;
using MGH.Core.Application.Pipelines.Authorization;
using Library.Application.Features.Libraries.Profiles;
using Library.Application.Features.Libraries.Constants;

namespace Library.Application.Features.Libraries.Commands.CreateLibrary;

[Roles(LibraryOperationClaims.AddLibraries)]
public class CreateLibraryCommand : ICommand<Guid>
{
    public string Name { get; set; }
    public string Code { get; set; }
    public string Location { get; set; }
    public DistrictEnum District { get; set; }
    public DateTime RegistrationTime { get; set; }
}

public class CreateLibraryCommandHandler(
    IUow uow,
    IEventBus eventBus,
    IPublicLibraryFactory libraryFactory,
    ILibraryBusinessRules libraryBusinessRules) : ICommandHandler<CreateLibraryCommand, Guid>
{
    public async Task<Guid> Handle(CreateLibraryCommand command, CancellationToken cancellationToken)
    {
        await libraryBusinessRules.LibraryCodeMustBeUnique(command.Code);

        var library = libraryFactory.Create(command.Name, command.Code, command.Location,
            command.RegistrationTime, command.District);

        await uow.Library.AddAsync(library);

        await eventBus.PublishAsync(library.ToLibraryCreatedDomainEvent(),
            PublishMode.Outbox,
            cancellationToken);
        await uow.CompleteAsync(cancellationToken);
        return library.Id;
    }
}