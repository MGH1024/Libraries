﻿using System.Collections;
using Domain.Security;
using MGH.Core.Domain.Entity.Base;
using MGH.Core.Infrastructure.Securities.Security.Entities;
using MGH.Core.Persistence.Extensions;
using MGH.Core.Persistence.Models.Filters.GetModels;
using MGH.Core.Persistence.Models.Paging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Persistence.Contexts;

namespace Persistence.Repositories.Security;

public class RefreshTokenRepository(LibraryDbContext libraryDbContext) : IRefreshTokenRepository
{
    public IQueryable<RefreshTkn> Query() => libraryDbContext.Set<RefreshTkn>();

    public async Task<RefreshTkn> GetAsync(GetModel<RefreshTkn> getBaseModel)
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

    public async Task<IPaginate<RefreshTkn>> GetListAsync(GetListAsyncModel<RefreshTkn> getListAsyncModel)
    {
        IQueryable<RefreshTkn> queryable = Query();
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
                .ToPaginateAsync(getListAsyncModel.Index, getListAsyncModel.Size, from: 0,
                    getListAsyncModel.CancellationToken);
        return await queryable.ToPaginateAsync(getListAsyncModel.Index, getListAsyncModel.Size, from: 0,
            getListAsyncModel.CancellationToken);
    }

    public async Task<IPaginate<RefreshTkn>> GetDynamicListAsync(GetDynamicListAsyncModel<RefreshTkn> dynamicGet)
    {
        IQueryable<RefreshTkn> queryable = Query().ToDynamic(dynamicGet.Dynamic);
        if (!dynamicGet.EnableTracking)
            queryable = queryable.AsNoTracking();
        if (dynamicGet.Include != null)
            queryable = dynamicGet.Include(queryable);
        if (dynamicGet.WithDeleted)
            queryable = queryable.IgnoreQueryFilters();
        if (dynamicGet.Predicate != null)
            queryable = queryable.Where(dynamicGet.Predicate);
        return await queryable.ToPaginateAsync(dynamicGet.Index, dynamicGet.Size, from: 0,
            dynamicGet.CancellationToken);
    }

    public async Task<RefreshTkn> AddAsync(RefreshTkn entity, CancellationToken cancellationToken)
    {
        await libraryDbContext.AddAsync(entity, cancellationToken);
        return entity;
    }

    public async Task<RefreshTkn> DeleteAsync(RefreshTkn entity, bool permanent = false)
    {
        await SetEntityAsDeletedAsync(entity, permanent);
        return entity;
    }

    public async Task<bool> AnyAsync(Base<RefreshTkn> @base)
    {
        IQueryable<RefreshTkn> queryable = Query();
        if (@base.EnableTracking)
            queryable = queryable.AsNoTracking();
        if (@base.WithDeleted)
            queryable = queryable.IgnoreQueryFilters();
        if (@base.Predicate != null)
            queryable = queryable.Where(@base.Predicate);
        return await queryable.AnyAsync(@base.CancellationToken);
    }

    public RefreshTkn Update(RefreshTkn entity, CancellationToken cancellationToken)
    {
        libraryDbContext.Update(entity);
        return entity;
    }

    public async Task<IEnumerable<RefreshTkn>> GetRefreshTokenByUserId(int userId, int refreshTokenTtl,
        CancellationToken cancellationToken)
    {
        var queryable = Query();
        queryable.Where(r =>
            r.UserId == userId
            && r.Revoked == null
            && r.Expires >= DateTime.UtcNow
            && r.CreatedAt.AddDays(refreshTokenTtl) <= DateTime.UtcNow);

        return await queryable.ToListAsync(cancellationToken);
    }

    public IEnumerable<RefreshTkn> DeleteRange(IEnumerable<RefreshTkn> entities, bool permanent = false)
    {
        SetEntitiesAsDeletedAsync(entities, permanent);
        return entities;
    }

    private async Task SetEntitiesAsDeletedAsync(IEnumerable<RefreshTkn> entities, bool permanent)
    {
        foreach (var entity in entities)
            await SetEntityAsDeletedAsync(entity, permanent);
    }

    private async Task SetEntityAsDeletedAsync(RefreshTkn entity, bool permanent)
    {
        if (!permanent)
        {
            CheckHasEntityHaveOneToOneRelation(entity);
            await SetEntityAsSoftDeletedAsync(entity);
        }
        else
        {
            libraryDbContext.Remove(entity);
        }
    }

    private void CheckHasEntityHaveOneToOneRelation(RefreshTkn entity)
    {
        bool hasEntityHaveOneToOneRelation =
            libraryDbContext
                .Entry(entity)
                .Metadata.GetForeignKeys()
                .All(
                    x =>
                        x.DependentToPrincipal?.IsCollection == true
                        || x.PrincipalToDependent?.IsCollection == true
                        || x.DependentToPrincipal?.ForeignKey.DeclaringEntityType.ClrType == entity.GetType()
                ) == false;
        if (hasEntityHaveOneToOneRelation)
            throw new InvalidOperationException(
                "Entity has one-to-one relationship. Soft Delete causes problems" +
                " if you try to create entry again by same foreign key."
            );
    }

    private async Task SetEntityAsSoftDeletedAsync(IAuditAbleEntity entity)
    {
        if (entity.DeletedAt.HasValue)
            return;
        entity.DeletedAt = DateTime.UtcNow;

        var navigations = libraryDbContext
            .Entry(entity)
            .Metadata.GetNavigations()
            .Where(x => x is
            {
                IsOnDependent: false, ForeignKey.DeleteBehavior: DeleteBehavior.ClientCascade or DeleteBehavior.Cascade
            })
            .ToList();
        foreach (INavigation navigation in navigations)
        {
            if (navigation.TargetEntityType.IsOwned())
                continue;
            if (navigation.PropertyInfo == null)
                continue;

            object navValue = navigation.PropertyInfo.GetValue(entity);
            if (navigation.IsCollection)
            {
                if (navValue == null)
                {
                    IQueryable query = libraryDbContext.Entry(entity).Collection(navigation.PropertyInfo.Name).Query();
                    navValue = await GetRelationLoaderQuery(query,
                        navigationPropertyType: navigation.PropertyInfo.GetType()).ToListAsync();
                }

                foreach (IAuditAbleEntity navValueItem in (IEnumerable)navValue)
                    await SetEntityAsSoftDeletedAsync(navValueItem);
            }
            else
            {
                if (navValue == null)
                {
                    IQueryable query = libraryDbContext.Entry(entity).Reference(navigation.PropertyInfo.Name).Query();
                    navValue = await GetRelationLoaderQuery(query,
                            navigationPropertyType: navigation.PropertyInfo.GetType())
                        .FirstOrDefaultAsync();
                    if (navValue == null)
                        continue;
                }

                await SetEntityAsSoftDeletedAsync((IAuditAbleEntity)navValue);
            }
        }

        libraryDbContext.Update(entity);
    }

    private IQueryable<object> GetRelationLoaderQuery(IQueryable query, Type navigationPropertyType)
    {
        var queryProviderType = query.Provider.GetType();
        var createQueryMethod =
            queryProviderType
                .GetMethods()
                .First(m => m is { Name: nameof(query.Provider.CreateQuery), IsGenericMethod: true })
                .MakeGenericMethod(navigationPropertyType);
        var queryProviderQuery =
            (IQueryable<object>)createQueryMethod.Invoke(query.Provider,
                parameters: new object[] { query.Expression })!;
        return queryProviderQuery.Where(x => !((IAuditAbleEntity)x).DeletedAt.HasValue);
    }
}