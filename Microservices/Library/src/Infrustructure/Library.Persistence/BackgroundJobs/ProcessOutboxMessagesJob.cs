using Domain.Entities.Libraries.Constant;
using Domain.Entities.Libraries.Events;
using Domain.Entities.Libraries.ValueObjects;
using MediatR;
using MGH.Core.Domain.Outboxes;
using MGH.Core.Infrastructure.Public;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Persistence.Contexts;
using Quartz;

namespace Persistence.BackgroundJobs;

[DisallowConcurrentExecution]
public class ProcessOutboxMessagesJob(LibraryDbContext libraryDbContext, ISender sender, IDateTime dateTime)
    : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        var outboxMessages = await libraryDbContext
            .Set<OutboxMessage>()
            .Where(a => a.ProcessedAt == null)
            .Take(50)
            .ToListAsync(context.CancellationToken);

        foreach (var outboxMessage in outboxMessages)
        {
            object obj = new();
            if (outboxMessage.Type == "LibraryCreatedDomainEvent")
                obj = JsonConvert.DeserializeObject<LibraryCreatedDomainEvent>(outboxMessage.Content);    
            
            await sender.Send(obj, context.CancellationToken);
            outboxMessage.ProcessedAt = dateTime.IranNow;
        }
        await libraryDbContext.SaveChangesAsync(context.CancellationToken);
    }
}