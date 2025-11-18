using Library.Domain;
using MGH.Core.Domain.Buses.Commands;
using Library.Domain.Libraries.Events;
using MGH.Core.Infrastructure.EventBus;
using Library.Application.Features.Libraries.Rules;
using Library.Application.Features.Libraries.Profiles;
using MGH.Core.Infrastructure.ElasticSearch.ElasticSearch.Base;

namespace Library.Application.Features.Libraries.Commands.CreateLibraryEvent;

public class LibraryCreatedDomainEventHandler(
    IUow uow,
    IEventBus eventBus,
    IElasticSearch elasticSearch,
    ILibraryBusinessRules businessRules)
    : ICommandHandler<LibraryCreatedDomainEvent>
{
    public async Task Handle(LibraryCreatedDomainEvent command, CancellationToken cancellationToken)
    {
        await businessRules.LibraryCreatedDomainEventShouldBeExist(command);

        await uow.BeginTransactionAsync(cancellationToken);
        try
        {
            await eventBus.PublishAsync(command, PublishMode.Outbox, cancellationToken);

            var elasticSearchInsertUpdateModel = command.ToElasticSearchInsertUpdateModel();
            var elasticSearchResult = await elasticSearch.InsertAsync(elasticSearchInsertUpdateModel);
            await businessRules.LibraryCreatedEventShouldBeRaisedInElk(elasticSearchResult);

            await uow.CommitTransactionAsync(cancellationToken);
        }
        catch (Exception)
        {
            await uow.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }
}