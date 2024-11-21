using System.Collections;
using Domain.Repositories;
using MGH.Core.Domain.BaseEntity.Abstract;
using MGH.Core.Infrastructure.Persistence.EF.Base.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Persistence.Contexts;
using MGH.Core.Infrastructure.Securities.Security.Entities;

namespace Persistence.Repositories.Security;

public class RefreshTokenRepository(SecurityDbContext securityDbContext)
    : Repository<RefreshTkn, int>(securityDbContext), IRefreshTokenRepository
{
    public IQueryable<RefreshTkn> Query() => securityDbContext.Set<RefreshTkn>();


    public async Task<IEnumerable<RefreshTkn>> GetRefreshTokenByUserId(int userId, int refreshTokenTtl,
        CancellationToken cancellationToken)
    {
        var queryable = Query();
        var refreshTokens = queryable.Where(r =>
            r.UserId == userId
            && r.Revoked == null
            && r.Expires >= DateTime.UtcNow
            && r.CreatedAt.AddDays(refreshTokenTtl) <= DateTime.UtcNow);

        return await queryable.ToListAsync(cancellationToken);
    }

    public async Task DeleteRangeAsync(IEnumerable<RefreshTkn> entities, bool permanent = false)
    {
        foreach (var refreshTkn in entities)
        {
            await this.DeleteAsync(refreshTkn, false);
        }
    }

    private async Task SetEntitiesAsDeletedAsync(IEnumerable<RefreshTkn> entities, bool permanent,
        CancellationToken cancellationToken)
    {
        foreach (var entity in entities)
            await SetEntityAsDeletedAsync(entity, permanent, cancellationToken);
    }

    private async Task SetEntityAsDeletedAsync(RefreshTkn entity, bool permanent, CancellationToken cancellationToken)
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

    private void CheckHasEntityHaveOneToOneRelation(RefreshTkn entity)
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
                        .FirstOrDefaultAsync();
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