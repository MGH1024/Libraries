using Library.Domain.Libraries;
using Library.Infrastructure.Contexts;
using MGH.Core.Domain.Entities;
using MGH.Core.Infrastructure.Persistence.EF.Base.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.Repositories;

public class LibraryRepository :
    Repository<Domain.Libraries.Library, Guid>,
    ILibraryRepository
{
    private readonly LibraryDbContext _libraryDbContext;
    public LibraryRepository(LibraryDbContext libraryDbContext)
        : base(libraryDbContext) 
    {
        _libraryDbContext = libraryDbContext;
    } 

    public IQueryable<Domain.Libraries.Library> Query() =>
       _libraryDbContext.Set<Domain.Libraries.Library>();
    public async Task<Domain.Libraries.Library> GetByCodeAsync(string code, CancellationToken cancellationToken)
    {
        return await Query()
            .FirstOrDefaultAsync(
                predicate: a => a.Code == code,
                cancellationToken: cancellationToken);
    }
}