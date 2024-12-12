using Domain.Entities.Libraries;
using Domain.Entities.Libraries.Events;
using MGH.Core.Infrastructure.ElasticSearch.ElasticSearch.Models;

namespace Application.Features.Libraries.Rules;

public interface ILibraryBusinessRules
{
    Task LibraryCodeMustBeUnique(string code);
    Task LibraryShouldBeExistsWhenSelected(Library library);
    Task LibraryCreatedEventShouldBeRaisedInElk(IElasticSearchResult elasticsearchResponse);
    Task LibraryCreatedDomainEventShouldBeExist(LibraryCreatedDomainEvent libraryCreatedDomainEvent);
}