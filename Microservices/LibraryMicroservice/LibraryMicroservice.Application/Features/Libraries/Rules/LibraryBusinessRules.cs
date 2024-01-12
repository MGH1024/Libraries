using Domain.Entities.Libraries;
using MGH.Core.Application.Rules;
using MGH.Core.CrossCutting.Exceptions.Types;

namespace Application.Features.Libraries.Rules;

public class LibraryBusinessRules : BaseBusinessRules
{
    private readonly ILibraryRepository _libraryRepository;

    public LibraryBusinessRules(ILibraryRepository libraryRepository)
    {
        _libraryRepository = libraryRepository;
    }
    public async Task LibraryCodeMustBeUnique(string code)
    {
        var library = await _libraryRepository.GetAsync(a => a.LibraryCode == code);
        if (library is not null)
            throw new BadRequestException("library code must be unique");
    }

    public Task LibraryShouldBeExistsWhenSelected(Library library)
    {
        if (library is null)
            throw new BusinessException("library not found");
        return Task.CompletedTask;
    }
}