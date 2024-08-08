using System.Linq.Expressions;
using Domain.Entities.Libraries;
using MGH.Core.Domain.Outboxes;
using MGH.Core.Infrastructure.Persistence.Persistence.Extensions;
using MGH.Core.Infrastructure.Persistence.Persistence.Models.Filters;
using MGH.Core.Infrastructure.Persistence.Persistence.Models.Paging;
using MGH.Core.Infrastructure.Public;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Persistence.Contexts;

namespace Persistence.Repositories;

public class OutBoxRepository(LibraryDbContext libraryDbContext , IDateTime dateTime) : IOutBoxRepository
{
    public IQueryable<OutboxMessage> Query() => libraryDbContext.Set<OutboxMessage>();
    
    public async Task<OutboxMessage> GetAsync(GetBaseModel<OutboxMessage> getBaseModel)
    {
        var queryable = Query();
        if (!getBaseModel.EnableTracking)
            queryable = queryable.AsNoTracking();
        if (getBaseModel.Include != null)
            queryable = getBaseModel.Include(queryable);
        if (getBaseModel.WithDeleted)
            queryable = queryable.IgnoreQueryFilters();
        return await queryable.FirstOrDefaultAsync(getBaseModel.Predicate, getBaseModel.CancellationToken);
    }
    
    public async Task<IPaginate<OutboxMessage>> GetListAsync(GetListAsyncModel<OutboxMessage> getListAsyncModel)
    {
        IQueryable<OutboxMessage> queryable = Query();
        if (!getListAsyncModel.EnableTracking)
            queryable = queryable.AsNoTracking();
        if (getListAsyncModel.Include != null)
            queryable = getListAsyncModel.Include(queryable);
        if (getListAsyncModel.WithDeleted)
            queryable = queryable.IgnoreQueryFilters();
        if (getListAsyncModel.Predicate != null)
            queryable = queryable.Where(getListAsyncModel.Predicate);
        if (getListAsyncModel.OrderBy != null)
            return await getListAsyncModel.OrderBy(queryable)
                .ToPaginateAsync(getListAsyncModel.Index, getListAsyncModel.Size, from: 0, getListAsyncModel.CancellationToken);
        return await queryable
            .ToPaginateAsync(getListAsyncModel.Index, getListAsyncModel.Size, from: 0, getListAsyncModel.CancellationToken);
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