using MGH.Core.Persistence.Repositories;
using System.Linq.Expressions;
using MGH.Core.Persistence.Dynamic;
using MGH.Core.Persistence.Paging;
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

    Task<IPaginate<Library>> GetListByDynamicAsync(
        DynamicQuery dynamic,
        Expression<Func<Library, bool>> predicate = null,
        Func<IQueryable<Library>, IIncludableQueryable<Library, object>> include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );

    Task<bool> AnyAsync(
        Expression<Func<Library, bool>> predicate = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );

    Task<Library> AddAsync(Library entity,CancellationToken cancellationToken);

    Task<ICollection<Library>> AddRangeAsync(ICollection<Library> entity);

    Library Update(Library entity);

    ICollection<Library> UpdateRange(ICollection<Library> entity);

    Task<Library> DeleteAsync(Library entity, bool permanent = false);

    Task<ICollection<Library>> DeleteRangeAsync(ICollection<Library> entity, bool permanent = false);
}