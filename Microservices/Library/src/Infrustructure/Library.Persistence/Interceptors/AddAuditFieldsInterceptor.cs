using MGH.Core.Domain.Abstracts;
using MGH.Core.Infrastructure.Public;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Persistence.Extensions;

namespace Persistence.Interceptors;

public class AddAuditFieldsInterceptor(IDateTime dateTime) : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
        InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        var now = dateTime.IranNow;
        var userName = "admin";
        if (eventData.Context != null)
        {
            var modifiedEntries = eventData.Context.ChangeTracker.Entries<IAuditable>().ToList();
            foreach (var item in modifiedEntries)
            {
                var entityType = item.Context.Model.FindEntityType(item.Entity.GetType());
                if (entityType is null)
                    continue;

                if (item.State == EntityState.Added)
                    item.AttachAddedState(now, userName);


                if (item.State == EntityState.Modified)
                    item.AttachModifiedState(now, userName);


                if (item.State == EntityState.Deleted)
                    item.AttachDeletedState(now, userName);
            }
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}