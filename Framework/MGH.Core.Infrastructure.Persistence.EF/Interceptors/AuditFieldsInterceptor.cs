﻿using MGH.Core.Infrastructure.Persistence.EF.Extensions;
using MGH.Core.Infrastructure.Public;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace MGH.Core.Infrastructure.Persistence.EF.Interceptors;

public class AuditFieldsInterceptor(IDateTime dateTime,string currentUserName) : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
        InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        var now = dateTime.IranNow;
        var userName = currentUserName ?? string.Empty;
        var dbContext = eventData.Context;
        if (dbContext is null)
            return base.SavingChangesAsync(eventData, result, cancellationToken);

        if (eventData.Context == null) 
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        
        eventData.SetAuditEntries( now, userName);
        eventData.SetOutbox(dbContext);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}