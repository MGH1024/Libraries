using MGH.Core.Domain.Events;
using Library.Domain.Outboxes;
using MGH.Core.Infrastructure.EventBus;

namespace Library.Endpoint.Worker.Outbox;

public class OutBoxWorker(IServiceProvider serviceProvider) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        using var scope = serviceProvider.CreateScope();
        var repo = scope.ServiceProvider.GetRequiredService<IOutboxMessageRepository>();
        var eventBus = scope.ServiceProvider.GetRequiredService<IEventBus>();

        while (!cancellationToken.IsCancellationRequested)
        {
            var outBoxList = await repo.GetListAsync();
            if (!outBoxList.Any())
            {
                await Task.Delay(1000,cancellationToken);
                continue;
            }

            var groupedOutboxItems = outBoxList.GroupBy(item => item.Type).ToList();
            foreach (var group in groupedOutboxItems)
            {
                var type = GetOutBoxType(group.Key);
                if (type is null)
                    throw new Exception("type is null");

                var events = group
                   .Select(g => g.DeserializePayloadAs<IEvent>())
                   .ToList();

                if (!events.Any())
                    throw new ApplicationException($"No content found for type {type}");

                await eventBus.PublishAsync(events, PublishMode.Direct, cancellationToken);

                foreach (var outbox in group)
                    outbox.MarkAsProcessed();

                await repo.UpdateRangeAsync(group.ToList(), cancellationToken);
            }
        }

        await Task.Delay(1000, cancellationToken);
    }

    private static Type? GetOutBoxType(string itemType)
    {
        return AppDomain.CurrentDomain
            .GetAssemblies()
            .Select(assembly => assembly.GetType(itemType))
            .FirstOrDefault(t => t != null);
    }
}