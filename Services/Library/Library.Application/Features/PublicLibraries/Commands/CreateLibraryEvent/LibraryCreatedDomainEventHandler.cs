using Library.Domain;
using Library.Domain.Libraries.Events;
using MGH.Core.Infrastructure.EventBus;
using MGH.Core.Application.Buses.Commands;
using MGH.Core.Infrastructure.ElasticSearch.ElasticSearch.Base;
using Library.Application.Features.PublicLibraries.Profiles;
using Library.Application.Features.PublicLibraries.Rules;

namespace Library.Application.Features.PublicLibraries.Commands.CreateLibraryEvent;

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