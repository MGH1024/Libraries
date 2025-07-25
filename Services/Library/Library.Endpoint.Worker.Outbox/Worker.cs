using MediatR;
using System.Text.Json;
using MGH.Core.Domain.Events;
using MGH.Core.Infrastructure.EventBus;
using Library.Endpoint.Worker.Outbox.Profiles;

namespace Library.Endpoint.Worker.Outbox;

public class Worker(IServiceProvider serviceProvider) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        using var scope = serviceProvider.CreateScope();
        var sender = scope.ServiceProvider.GetRequiredService<ISender>();
        var eventBus = scope.ServiceProvider.GetRequiredService<IEventBus>();

        while (!cancellationToken.IsCancellationRequested)
        {
            var request = MappingProfiles.ToGetOutboxListQuery();
            var outBoxList = await sender.Send(request, cancellationToken);
            if (outBoxList.Count <= 0) continue;

            var groupedOutboxItems = outBoxList.Items.GroupBy(item => item.Type).ToList();
            foreach (var group in groupedOutboxItems)
            {
                var type = GetOutBoxType(group.Key);
                if (type == null)
                    throw new Exception("type is null");
                
                var items = group.Select(x => x.Content).ToList();
                var deserializedContent = items.Select(content => JsonSerializer.Deserialize(content, type)).ToList();
                if (!deserializedContent.Any())
                    throw new ApplicationException($"No content found for type {type}");
                
                var events = deserializedContent.Cast<IEvent>().ToList();
                eventBus.Publish(events);
            }
            var updateProcessAtCommand = outBoxList.ToUpdateProcessAtCommand();
            await sender.Send(updateProcessAtCommand, cancellationToken);
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