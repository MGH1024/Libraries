using System.Linq.Expressions;
using MGH.Core.Infrastructure.Persistence.Base;
using MGH.Core.Infrastructure.Persistence.Models;
using MGH.Core.Infrastructure.Persistence.Models.Filters;
using MGH.Core.Infrastructure.Persistence.Models.Paging;
using Microsoft.EntityFrameworkCore.Query;

namespace Domain.Entities.Libraries;

public interface ILibraryRepository : IQuery<Library>
{
    Task<Library> GetAsync(
        Expression<Func<Library, bool>> predicate,
        Func<IQueryable<Library>, IIncludableQueryable<Library, object>> include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );
    Task<IPaginate<Library>> GetListAsync(
        Expression<Func<Library, bool>> predicate = null,
        Func<IQueryable<Library>, IOrderedQueryable<Library>> orderBy = null,
        Func<IQueryable<Library>, IIncludableQueryable<Library, object>> include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );
    Task<Library> AddAsync(Library entity,CancellationToken cancellationToken);
    Task<Library> DeleteAsync(Library entity, bool permanent = false);
    
}