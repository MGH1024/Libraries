using Library.Domain;
using MGH.Core.Domain.Buses.Commands;
using Library.Domain.Libraries.Constant;
using Library.Domain.Libraries.Factories;
using Library.Application.Features.Libraries.Rules;
using MGH.Core.Application.Pipelines.Authorization;
using Library.Application.Features.Libraries.Constants;

namespace Library.Application.Features.Libraries.Commands.CreateLibrary;

[Roles(LibraryOperationClaims.AddLibraries)]
public class CreateLibraryCommand : ICommand<Guid>
{
    public string Name { get; set; }
    public string Code { get; set; }
    public string Location { get; set; }
    public DistrictEnum DistrictEnum { get; set; }
    public DateTime RegistrationDate { get; set; }
}

public class CreateLibraryCommandHandler(
    ILibraryFactory libraryFactory,
    IUow uow,
    ILibraryBusinessRules libraryBusinessRules) : ICommandHandler<CreateLibraryCommand, Guid>
{
    public async Task<Guid> Handle(CreateLibraryCommand command, CancellationToken cancellationToken)
    {
        await libraryBusinessRules.LibraryCodeMustBeUnique(command.Code);

        var library = libraryFactory.Create(command.Name, command.Code, command.Location,
            command.RegistrationDate, command.DistrictEnum);
        await uow.Library.AddAsync(library);
        await uow.CompleteAsync(cancellationToken);

        // sender.Publish(new PublishModel<Library>
        // {
        //     Item = library,
        //     ExchangeName = "mgh-exchange",
        //     RoutingKey = "mgh-routingkey",
        //     QueueName = "mgh-queue",
        //     ExchangeType = "direct",
        // });


        // sender.Publish(new PublishList<Library>(new List<Library>()
        // {
        //     library, library, library
        // })
        // {
        //     ExchangeName = "mgh-exchange",
        //     RoutingKey = "mgh-routingkey",
        //     QueueName = "mgh-queue",
        //     ExchangeType = "direct",
        // });


        return library.Id;
    }
}