﻿using System.Collections;
using Domain.Entities.Security;
using MGH.Core.Domain.Entity.Base;
using MGH.Core.Infrastructure.Securities.Security.Entities;
using MGH.Core.Persistence.Extensions;
using MGH.Core.Persistence.Models.Filters.GetModels;
using MGH.Core.Persistence.Models.Paging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Persistence.Contexts;

namespace Persistence.Repositories.Security;

public class OtpAuthenticatorRepository(SecurityDbContext securityDbContext) : IOtpAuthenticatorRepository
{
    public IQueryable<OtpAuthenticator> Query() => securityDbContext.Set<OtpAuthenticator>();

    public async Task<OtpAuthenticator> GetAsync(GetModel<OtpAuthenticator> getBaseModel)
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

    public async Task<IPaginate<OtpAuthenticator>> GetListAsync(GetListAsyncModel<OtpAuthenticator> getListAsyncModel)
    {
        IQueryable<OtpAuthenticator> queryable = Query();
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

    public async Task<IPaginate<OtpAuthenticator>> GetDynamicListAsync(
        GetDynamicListAsyncModel<OtpAuthenticator> dynamicGet)
    {
        IQueryable<OtpAuthenticator> queryable = Query().ToDynamic(dynamicGet.Dynamic);
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

    public async Task<OtpAuthenticator> AddAsync(OtpAuthenticator entity, CancellationToken cancellationToken)
    {
        await securityDbContext.AddAsync(entity, cancellationToken);
        return entity;
    }

    public async Task<OtpAuthenticator> DeleteAsync(OtpAuthenticator entity, bool permanent = false,
        CancellationToken cancellationToken = default)
    {
        await SetEntityAsDeletedAsync(entity, permanent, cancellationToken);
        return entity;
    }

    public async Task<OtpAuthenticator> UpdateAsync(OtpAuthenticator entity, CancellationToken cancellationToken)
    {
        await Task.Run(() => securityDbContext.Update(entity), cancellationToken);
        return entity;
    }

    private async Task SetEntityAsDeletedAsync(OtpAuthenticator entity, bool permanent,
        CancellationToken cancellationToken)
    {
        if (!permanent)
        {
            CheckHasEntityHaveOneToOneRelation(entity);
            await SetEntityAsSoftDeletedAsync(entity, cancellationToken);
        }
        else
        {
            securityDbContext.Remove(entity);
        }
    }

    private void CheckHasEntityHaveOneToOneRelation(OtpAuthenticator entity)
    {
        bool hasEntityHaveOneToOneRelation =
            securityDbContext
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

    private async Task SetEntityAsSoftDeletedAsync(IAuditAbleEntity entity, CancellationToken cancellationToken)
    {
        if (entity.DeletedAt.HasValue)
            return;
        entity.DeletedAt = DateTime.UtcNow;

        var navigations = securityDbContext
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
                    IQueryable query = securityDbContext.Entry(entity).Collection(navigation.PropertyInfo.Name).Query();
                    navValue = await GetRelationLoaderQuery(query,
                        navigationPropertyType: navigation.PropertyInfo.GetType()).ToListAsync(cancellationToken);
                }

                foreach (IAuditAbleEntity navValueItem in (IEnumerable)navValue)
                    await SetEntityAsSoftDeletedAsync(navValueItem, cancellationToken);
            }
            else
            {
                if (navValue == null)
                {
                    IQueryable query = securityDbContext.Entry(entity).Reference(navigation.PropertyInfo.Name).Query();
                    navValue = await GetRelationLoaderQuery(query,
                            navigationPropertyType: navigation.PropertyInfo.GetType())
                        .FirstOrDefaultAsync(cancellationToken);
                    if (navValue == null)
                        continue;
                }

                await SetEntityAsSoftDeletedAsync((IAuditAbleEntity)navValue, cancellationToken);
            }
        }

        securityDbContext.Update(entity);
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