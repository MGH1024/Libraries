using Library.Domain.Libraries;
using MGH.Core.Application.Rules;
using Library.Domain.Libraries.Events;
using MGH.Core.CrossCutting.Exceptions.Types;
using MGH.Core.Infrastructure.ElasticSearch.ElasticSearch.Models;

namespace Library.Application.Features.PublicLibraries.Rules;

public  class LibraryBusinessRules(IPublicLibraryRepository libraryRepository) : BaseBusinessRules, ILibraryBusinessRules
{
    public async Task LibraryCodeMustBeUnique(string code)
    {
        var library = await libraryRepository.GetByCodeAsync(code);
        if (library is not null)
            throw new BusinessException("library code must be unique");
    }

    public Task LibraryCreatedEventShouldBeRaisedInElk(IElasticSearchResult elasticsearchResponse)
    {
        if(elasticsearchResponse is not null && !elasticsearchResponse.Success)
            throw new BusinessException("elastic search exception");
        return Task.CompletedTask;
    }

    public Task LibraryCreatedDomainEventShouldBeExist(LibraryCreatedDomainEvent libraryCreatedDomainEvent)
    {
        if(libraryCreatedDomainEvent is null)
            throw new BusinessException("LibraryCreatedDomainEvent is null");
        return Task.CompletedTask;
    }
}