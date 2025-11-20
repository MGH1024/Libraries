using Library.Domain.Libraries;
using Microsoft.EntityFrameworkCore;
using Library.Infrastructure.Contexts;
using MGH.Core.Infrastructure.Persistence.EF.Base.Repositories;

namespace Library.Infrastructure.Repositories;

public class PublicLibraryRepository(PublicLibraryDbContext _libraryDbContext) :
    Repository<PublicLibrary, Guid>(_libraryDbContext),
    IPublicLibraryRepository
{
    public IQueryable<PublicLibrary> Query() =>
        _libraryDbContext.Set<PublicLibrary>();

    public async Task<PublicLibrary> GetByCodeAsync(string code, CancellationToken cancellationToken)
    {
        return await Query()
            .FirstOrDefaultAsync(
                predicate: a => a.Code == code,
                cancellationToken: cancellationToken);
    }
}
