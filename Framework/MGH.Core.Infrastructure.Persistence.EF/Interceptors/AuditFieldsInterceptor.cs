using MGH.Core.Infrastructure.Persistence.EF.Extensions;
using MGH.Core.Infrastructure.Public;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace MGH.Core.Infrastructure.Persistence.EF.Interceptors;

public class AuditFieldsInterceptor(IDateTime dateTime,IHttpContextAccessor httpContextAccessor ) : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
        InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        var now = dateTime.IranNow;
        var userName = httpContextAccessor.HttpContext?.User?.Identity?.Name ?? string.Empty;
        var dbContext = eventData.Context;
        if (dbContext is null)
            return base.SavingChangesAsync(eventData, result, cancellationToken);

        if (eventData.Context == null) 
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        
        eventData.SetAuditEntries( now, userName);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}