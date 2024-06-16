using Domain.Entities.Libraries.Events;
using MGH.Core.Domain.Buses.Commands;
using MGH.Core.Infrastructure.ElasticSearch;
using MGH.Core.Infrastructure.ElasticSearch.Base;
using MGH.Core.Infrastructure.ElasticSearch.Models;

namespace Application.Features.Libraries.Commands.CreateLibraryEvent;

public class LibraryCreatedDomainEventHandler(IElasticSearch elasticSearch) : ICommandHandler<LibraryCreatedDomainEvent>
{
    public async Task Handle(LibraryCreatedDomainEvent command, CancellationToken cancellationToken)
    {
        await elasticSearch.InsertAsync(new ElasticSearchInsertUpdateModel(command)
        {
            IndexName = "libraries",
            ElasticId = command.Id
        });
    }
}