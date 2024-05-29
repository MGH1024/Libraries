using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace MGH.Core.Infrastructure.Persistence.Extensions;

public static class AddAuditFieldsInterceptorExtension
{
    public static void AttachAddedState(this EntityEntry item, DateTime now, string userName)
    {
        item.Property("CreatedAt").CurrentValue = now;
        item.Property("CreatedBy").CurrentValue = userName;
    }

    public static void AttachDeletedState(this EntityEntry item, DateTime now, string userName)
    {
        item.Property("DeletedAt").CurrentValue = now;
        item.Property("DeletedBy").CurrentValue = userName;
    }

    public static void AttachModifiedState(this EntityEntry item, DateTime now, string userName)
    {
        item.Property("UpdatedAt").CurrentValue = now;
        item.Property("UpdatedBy").CurrentValue = userName;
    }
}