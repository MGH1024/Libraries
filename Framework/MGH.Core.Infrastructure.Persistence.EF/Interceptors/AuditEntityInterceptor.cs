using System.Text.Json;
using MGH.Core.Domain.Entity.Logs;
using Microsoft.EntityFrameworkCore;
using MGH.Core.Infrastructure.Public;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace MGH.Core.Infrastructure.Persistence.EF.Interceptors;

public class AuditEntityInterceptor(IDateTime dateTime, string currentUserName) : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        var context = eventData.Context;
        if (context == null) return base.SavingChanges(eventData, result);

        var auditLogs = new List<AuditLog>();

        foreach (var entry in context.ChangeTracker.Entries())
        {
            if (entry.Entity is AuditLog || entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
                continue;

            var auditLog = new AuditLog
            {
                Id = Guid.NewGuid(),
                TableName = entry.Entity.GetType().Name,
                Action = entry.State.ToString(),
                Username = currentUserName,
                Timestamp = dateTime.IranNow,
            };

            if (entry.State == EntityState.Modified)
            {
                auditLog.BeforeData = JsonSerializer.Serialize(entry.OriginalValues.Properties.ToDictionary(
                    p => p.Name,
                    p => entry.OriginalValues[p]?.ToString()
                ));

                auditLog.AfterData = JsonSerializer.Serialize(entry.CurrentValues.Properties.ToDictionary(
                    p => p.Name,
                    p => entry.CurrentValues[p]?.ToString()
                ));
            }
            else if (entry.State == EntityState.Added)
            {
                auditLog.BeforeData = null;
                auditLog.AfterData = JsonSerializer.Serialize(entry.CurrentValues.Properties.ToDictionary(
                    p => p.Name,
                    p => entry.CurrentValues[p]?.ToString()
                ));
            }
            else if (entry.State == EntityState.Deleted)
            {
                auditLog.BeforeData = JsonSerializer.Serialize(entry.OriginalValues.Properties.ToDictionary(
                    p => p.Name,
                    p => entry.OriginalValues[p]?.ToString()
                ));
                auditLog.AfterData = null;
            }

            auditLogs.Add(auditLog);
        }

        context.AddRange(auditLogs);
        return base.SavingChanges(eventData, result);
    }
}