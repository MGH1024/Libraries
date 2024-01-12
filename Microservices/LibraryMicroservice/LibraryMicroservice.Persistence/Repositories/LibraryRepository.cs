﻿using System.Collections;
using System.Linq.Expressions;
using Domain.Entities.Libraries;
using MGH.Core.Domain.Abstracts;
using MGH.Core.Persistence.Dynamic;
using MGH.Core.Persistence.Paging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Query;
using Persistence.Contexts;

namespace Persistence.Repositories;

public class LibraryRepository : ILibraryRepository
{
    private readonly LibraryDbContext _libraryDbContext;

    public LibraryRepository(LibraryDbContext libraryDbContext)
        => _libraryDbContext = libraryDbContext;

    public IQueryable<Library> Query() => _libraryDbContext.Set<Library>();

    public async Task<Library> GetAsync(
        Expression<Func<Library, bool>> predicate,
        Func<IQueryable<Library>, IIncludableQueryable<Library, object>> include = null,
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

    public async Task<IPaginate<Library>> GetListAsync(
        Expression<Func<Library, bool>> predicate = null,
        Func<IQueryable<Library>, IOrderedQueryable<Library>> orderBy = null,
        Func<IQueryable<Library>, IIncludableQueryable<Library, object>> include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default)
    {
        IQueryable<Library> queryable = Query();
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

    public async Task<IPaginate<Library>> GetListByDynamicAsync(
        DynamicQuery dynamic,
        Expression<Func<Library, bool>> predicate = null,
        Func<IQueryable<Library>, IIncludableQueryable<Library, object>> include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default)
    {
        IQueryable<Library> queryable = Query().ToDynamic(dynamic);
        if (!enableTracking)
            queryable = queryable.AsNoTracking();
        if (include != null)
            queryable = include(queryable);
        if (withDeleted)
            queryable = queryable.IgnoreQueryFilters();
        if (predicate != null)
            queryable = queryable.Where(predicate);
        return await queryable.ToPaginateAsync(index, size, from: 0, cancellationToken);
    }

    public async Task<bool> AnyAsync(
        Expression<Func<Library, bool>> predicate = null,
        bool withDeleted = false, 
        bool enableTracking = true,
        CancellationToken cancellationToken = default)
    {
        IQueryable<Library> queryable = Query();
        if (!enableTracking)
            queryable = queryable.AsNoTracking();
        if (withDeleted)
            queryable = queryable.IgnoreQueryFilters();
        if (predicate != null)
            queryable = queryable.Where(predicate);
        return await queryable.AnyAsync(cancellationToken);
    }

    public async Task<Library> AddAsync(Library entity, CancellationToken cancellationToken)
    {
        await _libraryDbContext.AddAsync(entity, cancellationToken);
        return entity;
    }

    public async Task<ICollection<Library>> AddRangeAsync(ICollection<Library> entity)
    {
        await _libraryDbContext.AddRangeAsync(entity);
        return entity;
    }

    public Library  Update(Library entity)
    {
        _libraryDbContext.Update(entity);
        return entity;
    }

    public ICollection<Library> UpdateRange(ICollection<Library> entity)
    {
        _libraryDbContext.UpdateRange(entity);
        return entity;
    }

    public async Task<Library> DeleteAsync(Library entity, bool permanent = false)
    {
        await SetEntityAsDeletedAsync(entity, permanent);
        return entity;
    }

    public Task<ICollection<Library>> DeleteRangeAsync(ICollection<Library> entity, bool permanent = false)
    {
        throw new NotImplementedException();
    }

    private async Task SetEntityAsDeletedAsync(Library entity, bool permanent)
    {
        if (!permanent)
        {
            CheckHasEntityHaveOneToOneRelation(entity);
            await SetEntityAsSoftDeletedAsync(entity);
        }
        else
        {
            _libraryDbContext.Remove(entity);
        }
    }

    private void CheckHasEntityHaveOneToOneRelation(Library entity)
    {
        bool hasEntityHaveOneToOneRelation =
            _libraryDbContext
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

    private async Task SetEntityAsSoftDeletedAsync(IAuditable entity)
    {
        if (entity.DeletedAt.HasValue)
            return;
        entity.DeletedAt = DateTime.UtcNow;

        var navigations = _libraryDbContext
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
                    IQueryable query = _libraryDbContext.Entry(entity).Collection(navigation.PropertyInfo.Name).Query();
                    navValue = await GetRelationLoaderQuery(query,
                        navigationPropertyType: navigation.PropertyInfo.GetType()).ToListAsync();
                }

                foreach (IAuditable navValueItem in (IEnumerable)navValue)
                    await SetEntityAsSoftDeletedAsync(navValueItem);
            }
            else
            {
                if (navValue == null)
                {
                    IQueryable query = _libraryDbContext.Entry(entity).Reference(navigation.PropertyInfo.Name).Query();
                    navValue = await GetRelationLoaderQuery(query,
                            navigationPropertyType: navigation.PropertyInfo.GetType())
                        .FirstOrDefaultAsync();
                    if (navValue == null)
                        continue;
                }

                await SetEntityAsSoftDeletedAsync((IAuditable)navValue);
            }
        }

        _libraryDbContext.Update(entity);
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
        return queryProviderQuery.Where(x => !((IAuditable)x).DeletedAt.HasValue);
    }
}