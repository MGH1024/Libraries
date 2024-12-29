using Library.Domain.Libraries.Events;
using MGH.Core.Infrastructure.ElasticSearch.ElasticSearch.Models;

namespace Library.Application.Features.Libraries.Rules;

public interface ILibraryBusinessRules
{
    Task LibraryCodeMustBeUnique(string code);
    Task LibraryShouldBeExistsWhenSelected(Domain.Libraries.Library library);
    Task LibraryCreatedEventShouldBeRaisedInElk(IElasticSearchResult elasticsearchResponse);
    Task LibraryCreatedDomainEventShouldBeExist(LibraryCreatedDomainEvent libraryCreatedDomainEvent);
}