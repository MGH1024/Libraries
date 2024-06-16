using System.Linq.Expressions;
using Domain.Entities.Libraries;
using MGH.Core.Domain.Outboxes;
using MGH.Core.Infrastructure.Persistence.Persistence.Extensions;
using MGH.Core.Infrastructure.Persistence.Persistence.Models.Paging;
using MGH.Core.Infrastructure.Public;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Persistence.Contexts;

namespace Persistence.Repositories;

public class OutBoxRepository(LibraryDbContext libraryDbContext , IDateTime dateTime) : IOutBoxRepository
{
    public IQueryable<OutboxMessage> Query() => libraryDbContext.Set<OutboxMessage>();

    
    
    public async Task<OutboxMessage> GetAsync(
        Expression<Func<OutboxMessage, bool>> predicate,
        Func<IQueryable<OutboxMessage>, IIncludableQueryable<OutboxMessage, object>> include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default)
    {
        var queryable = Query();
        if (!enableTracking)
            queryable = queryable.AsNoTracking();
        if (include != null)
            queryable = include(queryable);
        if (withDeleted)
            queryable = queryable.IgnoreQueryFilters();
        return await queryable.FirstOrDefaultAsync(predicate, cancellationToken);
    }
    
    public async Task<IPaginate<OutboxMessage>> GetListAsync(Expression<Func<OutboxMessage, bool>> predicate = null,
        Func<IQueryable<OutboxMessage>, IOrderedQueryable<OutboxMessage>> orderBy = null,
        Func<IQueryable<OutboxMessage>, IIncludableQueryable<OutboxMessage, object>> include = null, int index = 0,
        int size = 10, bool withDeleted = false, bool enableTracking = true,
        CancellationToken cancellationToken = default)
    {
        IQueryable<OutboxMessage> queryable = Query();
        if (!enableTracking)
            queryable = queryable.AsNoTracking();
        if (include != null)
            queryable = include(queryable);
        if (withDeleted)
            queryable = queryable.IgnoreQueryFilters();
        if (predicate != null)
            queryable = queryable.Where(predicate);
        if (orderBy != null)
            return await orderBy(queryable).ToPaginateAsync(index, size, from: 0, cancellationToken);
        return await queryable.ToPaginateAsync(index, size, from: 0, cancellationToken);
    }
    
    public OutboxMessage  Update(OutboxMessage entity)
    {
        libraryDbContext.Update(entity);
        return entity;
    }

    public void Update(IEnumerable<Guid> idList)
    {
        var lstOutbox 
            = Query().Where(a => idList.Contains(a.Id)).ToList();
        
        lstOutbox.ForEach(x => x.ProcessedAt = dateTime.IranNow);
    }
}