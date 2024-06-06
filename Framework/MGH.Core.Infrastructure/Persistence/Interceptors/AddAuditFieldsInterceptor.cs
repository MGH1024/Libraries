using MGH.Core.Infrastructure.Public;
using Microsoft.EntityFrameworkCore.Diagnostics;
using MGH.Core.Infrastructure.Persistence.Extensions;

namespace MGH.Core.Infrastructure.Persistence.Interceptors;

public class AddAuditFieldsInterceptor(IDateTime dateTime) : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
        InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        var now = dateTime.IranNow;
        var userName = "admin";
        var dbContext = eventData.Context;
        if (dbContext is null)
            return base.SavingChangesAsync(eventData, result, cancellationToken);

        if (eventData.Context == null) 
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        
        eventData.SetAuditEntries( now, userName);
        eventData.SetOutbox( dbContext);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    
}