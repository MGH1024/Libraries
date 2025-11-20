using Library.Domain.Libraries;
using Library.Domain.Libraries.Events;
using MGH.Core.Infrastructure.ElasticSearch.ElasticSearch.Models;

namespace Library.Application.Features.Libraries.Rules;

public interface ILibraryBusinessRules
{
    Task LibraryCodeMustBeUnique(string code);
    Task LibraryShouldBeExistsWhenSelected(PublicLibrary library);
    Task LibraryCreatedEventShouldBeRaisedInElk(IElasticSearchResult elasticSearchResponse);
    Task LibraryCreatedDomainEventShouldBeExist(LibraryCreatedDomainEvent libraryCreatedDomainEvent);
}