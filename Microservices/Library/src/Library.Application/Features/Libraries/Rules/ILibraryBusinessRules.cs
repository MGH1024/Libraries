using Domain.Entities.Libraries;

namespace Application.Features.Libraries.Rules;

public interface ILibraryBusinessRules
{
    Task LibraryCodeMustBeUnique(string code);
    Task LibraryShouldBeExistsWhenSelected(Library library);
}