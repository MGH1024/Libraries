using Library.Domain;
using Library.Domain.Libraries.Events;
using MGH.Core.Infrastructure.EventBus;
using MGH.Core.Application.Buses.Commands;
using Library.Application.Features.PublicLibraries.Rules;
using Library.Application.Features.PublicLibraries.Profiles;
using MGH.Core.Infrastructure.ElasticSearch.ElasticSearch.Base;

namespace Library.Application.Features.PublicLibraries.Commands.AddEvent;

public class AddEventHandler(
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