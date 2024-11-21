using System.Text.Json;
using MGH.Core.Domain.BaseEntity.Abstract;
using MGH.Core.Domain.Entity.Outboxes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace MGH.Core.Infrastructure.Persistence.EF.Extensions;

public static class AddAuditFieldsInterceptorExtension
{
    private static void AttachAddedState(this EntityEntry item, DateTime now, string userName)
    {
        item.Property("CreatedAt").CurrentValue = now;
        item.Property("CreatedBy").CurrentValue = userName;
    }

    private static void AttachDeletedState(this EntityEntry item, DateTime now, string userName)
    {
        var deletedAtValue = item.Property("DeletedAt").CurrentValue;
        if (deletedAtValue is not null)
        {
            item.Property("DeletedAt").CurrentValue = now;
            item.Property("DeletedBy").CurrentValue = userName;
        }
    }

    private static void AttachModifiedState(this EntityEntry item, DateTime now, string userName)
    {
        item.Property("UpdatedAt").CurrentValue = now;
        item.Property("UpdatedBy").CurrentValue = userName;
    }

    public static void SetOutbox(this DbContextEventData eventData, DbContext dbContext)
    {
        var outboxMessages =
            eventData.Context?.ChangeTracker.Entries<IAggregateRoot>()
                .Select(a => a.Entity)
                .Where(a => a.Events.Any())
                .SelectMany(a => a.Events)
                .Select(a => new OutboxMessage
                {
                    Id = Guid.NewGuid(),
                    CreatedAt = DateTime.Now,
                    Type = a.GetType().Name,
                    Content = JsonSerializer.Serialize(a, new JsonSerializerOptions
                    {
                        WriteIndented = false,
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    })
                }).ToList();

        if (outboxMessages != null)
            dbContext.Set<OutboxMessage>().AddRange(outboxMessages);
    }

    public static void SetAuditEntries(this DbContextEventData eventData, DateTime now, string userName)
    {
        var modifiedEntries = eventData.Context?.ChangeTracker.Entries<IAuditAbleEntity>().ToList();
        if (modifiedEntries == null) return;
        foreach (var item in modifiedEntries)
        {
            var entityType = item.Context.Model.FindEntityType(item.Entity.GetType());
            if (entityType is null)
                continue;

            if (item.State == EntityState.Added)
                item.AttachAddedState(now, userName);
            
            if (item.State == EntityState.Deleted)
                item.AttachDeletedState(now, userName);
            
            if (item.State == EntityState.Modified)
            {
                item.AttachModifiedState(now, userName);
                item.AttachDeletedState(now, userName);
            }
        }
    }
}