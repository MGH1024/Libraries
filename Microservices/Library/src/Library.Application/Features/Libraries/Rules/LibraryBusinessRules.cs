using Application.Features.Libraries.Profiles;
using Domain.Entities.Libraries;
using Domain.Entities.Libraries.Events;
using MGH.Core.Application.Rules;
using MGH.Core.CrossCutting.Exceptions.Types;
using MGH.Core.Infrastructure.ElasticSearch.ElasticSearch.Models;

namespace Application.Features.Libraries.Rules;

public  class LibraryBusinessRules(ILibraryRepository libraryRepository) : BaseBusinessRules, ILibraryBusinessRules
{
    public async Task LibraryCodeMustBeUnique(string code)
    {
        var library = await libraryRepository.GetAsync(code.ToGetBaseLibraryModel());
        if (library is not null)
            throw new BusinessException("library code must be unique");
    }

    public Task LibraryShouldBeExistsWhenSelected(Library library)
    {
        if (library is null)
            throw new BusinessException("library not found");
        return Task.CompletedTask;
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