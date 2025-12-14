using Library.Domain;
using MGH.Core.Application.Buses;
using Library.Domain.Libraries.Events;
using MGH.Core.Infrastructure.EventBus;
using Library.Domain.Libraries.Exceptions;
using MGH.Core.CrossCutting.Exceptions.Types;
using MGH.Core.Infrastructure.ElasticSearch.ElasticSearch.Base;
using MGH.Core.Infrastructure.ElasticSearch.ElasticSearch.Models;

namespace Library.Application.Features.PublicLibraries.Commands.AddEvent;

public class AddEventHandler(
    IUow uow,
    IEventBus eventBus,
    IElasticSearch elasticSearch)
    : ICommandHandler<LibraryCreatedDomainEvent>
{
    public async Task Handle(LibraryCreatedDomainEvent command, CancellationToken cancellationToken)
    {
        if (command is null)
            throw new LibraryException("LibraryCreatedDomainEvent is null");

        await eventBus.PublishAsync(command, PublishMode.Outbox, cancellationToken);

        var elasticModel = ToElasticModel(command);
        var elasticSearchResult = await elasticSearch.InsertAsync(elasticModel);
        if (elasticSearchResult is not null && !elasticSearchResult.Success)
            throw new BusinessException("elastic search exception");

        await uow.CompleteAsync(cancellationToken);

    }

    private ElasticSearchInsertUpdateModel ToElasticModel(LibraryCreatedDomainEvent libraryCreatedDomainEvent)
    {
        return new ElasticSearchInsertUpdateModel(libraryCreatedDomainEvent)
        {
            IndexName = "libraries",
            ElasticId = libraryCreatedDomainEvent.Id
        };
    }
}