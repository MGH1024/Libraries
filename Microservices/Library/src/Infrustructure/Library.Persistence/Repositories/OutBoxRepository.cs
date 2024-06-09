﻿using System.Linq.Expressions;
using Domain.Entities.Libraries;
using MGH.Core.Domain.Outboxes;
using MGH.Core.Infrastructure.Persistence.Paging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Persistence.Contexts;

namespace Persistence.Repositories;

public class OutBoxRepository(LibraryDbContext libraryDbContext) : IOutBoxRepository
{
    public IQueryable<OutboxMessage> Query() => libraryDbContext.Set<OutboxMessage>();

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
}