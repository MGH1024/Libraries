using System.Linq.Expressions;
using MGH.Core.Domain.Entity.Base;
using MGH.Core.Infrastructure.Persistence.Models;
using MGH.Core.Infrastructure.Persistence.Paging;
using Microsoft.EntityFrameworkCore.Query;

namespace MGH.Core.Infrastructure.Persistence.Repositories;

public interface IRepository<TEntity, TEntityId> : IQuery<TEntity>
    where TEntity : AuditableEntity<TEntityId>
{
    TEntity Get(
        Expression<Func<TEntity, bool>> predicate,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
        bool withDeleted = false,
        bool enableTracking = true
    );

    Task<TEntity> GetAsync(
        Expression<Func<TEntity, bool>> predicate,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );

    IPaginate<TEntity> GetList(
        Expression<Func<TEntity, bool>> predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true
    );

    Task<IPaginate<TEntity>> GetListAsync(
        Expression<Func<TEntity, bool>> predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );

    IPaginate<TEntity> GetListByDynamic(
        DynamicQuery dynamic,
        Expression<Func<TEntity, bool>> predicate = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true
    );

    Task<IPaginate<TEntity>> GetListByDynamicAsync(
        DynamicQuery dynamic,
        Expression<Func<TEntity, bool>> predicate = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );

    bool Any(
        Expression<Func<TEntity, bool>> predicate = null,
        bool withDeleted = false,
        bool enableTracking = true);

    Task<bool> AnyAsync(
        Expression<Func<TEntity, bool>> predicate = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );

    TEntity Add(TEntity entity);
    
    Task<TEntity> AddAsync(TEntity entity);

    ICollection<TEntity> AddRange(ICollection<TEntity> entities);
    
    Task<ICollection<TEntity>> AddRangeAsync(ICollection<TEntity> entity);

    TEntity Update(TEntity entity);
    
    Task<TEntity> UpdateAsync(TEntity entity);
    
    ICollection<TEntity> UpdateRange(ICollection<TEntity> entities);
    
    Task<ICollection<TEntity>> UpdateRangeAsync(ICollection<TEntity> entity);

    TEntity Delete(TEntity entity, bool permanent = false);
    
    Task<TEntity> DeleteAsync(TEntity entity, bool permanent = false);
    
    ICollection<TEntity> DeleteRange(ICollection<TEntity> entity, bool permanent = false);
    
    Task<ICollection<TEntity>> DeleteRangeAsync(ICollection<TEntity> entity, bool permanent = false);
}