using Domain.Entities.Libraries;
using Domain.Entities.Libraries.Constant;
using Domain.Entities.Libraries.Factories;
using Application.Features.Libraries.Rules;
using MGH.Core.Domain.Buses.Commands;
using MGH.Core.Infrastructure.Persistence.UnitOfWork;

namespace Application.Features.Libraries.Commands.CreateLibrary;

public class CreateLibraryCommand : ICommand<Guid>
{
    public string Name { get; set; }
    public string Code { get; set; }
    public string Location { get; set; }
    public District District { get; set; }
    public DateTime RegistrationDate { get; set; }
}

public class CreateLibraryCommandHandler(
    ILibraryRepository libraryRepository,
    ILibraryFactory libraryFactory,
    IUnitOfWork unitOfWork,
    LibraryBusinessRules libraryBusinessRules)
    : ICommandHandler<CreateLibraryCommand, Guid>
{
    public async Task<Guid> Handle(CreateLibraryCommand command, CancellationToken cancellationToken)
    {
        await libraryBusinessRules.LibraryCodeMustBeUnique(command.Code);

        var library = libraryFactory.Create(command.Name, command.Code, command.Location,
            command.RegistrationDate, command.District);
        await libraryRepository.AddAsync(library, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);

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