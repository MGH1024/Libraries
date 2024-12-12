using Domain;
using MGH.Core.Domain.Buses.Commands;
using Domain.Entities.Libraries.Events;
using Application.Features.Libraries.Rules;
using Application.Features.Libraries.Profiles;
using MGH.Core.Infrastructure.ElasticSearch.ElasticSearch.Base;

namespace Application.Features.Libraries.Commands.CreateLibraryEvent;

public class LibraryCreatedDomainEventHandler(IElasticSearch elasticSearch, ILibraryBusinessRules businessRules, IUow uow) : ICommandHandler<LibraryCreatedDomainEvent>
{
    public async Task Handle(LibraryCreatedDomainEvent command, CancellationToken cancellationToken)
    {
        await businessRules.LibraryCreatedDomainEventShouldBeExist(command);
            
        await uow.BeginTransactionAsync(cancellationToken);
        try
        {
            var outbox = command.ToOutBox();
            await uow.OutBox.AddAsync(outbox, cancellationToken);

            var elasticSearchInsertUpdateModel = command.ToElasticSearchInsertUpdateModel();
            var elasticSearchResult = await elasticSearch.InsertAsync(elasticSearchInsertUpdateModel, cancellationToken);
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