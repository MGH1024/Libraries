using MediatR;
using System.Text.Json;
using MGH.Core.Application.Responses;
using MGH.Core.Infrastructure.MessageBroker;
using Library.Endpoint.Worker.Outbox.Profiles;
using MGH.Core.Domain.BaseEntity.Abstract.Events;
using Application.Features.OutBoxes.Queries.GetList;
using MGH.Core.Infrastructure.MessageBroker.RabbitMq.Attributes;

namespace Library.Endpoint.Worker.Outbox;

[BaseMessage("mgh-routingkey", "direct", "mgh-exchange", "mgh-queue")]
public class Worker(IServiceProvider serviceProvider) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        using var scope = serviceProvider.CreateScope();
        var sender = scope.ServiceProvider.GetRequiredService<ISender>();
        var eventBus = scope.ServiceProvider.GetRequiredService<IEventBusDispatcher>();

        while (!cancellationToken.IsCancellationRequested)
        {
            var request = MappingProfiles.ToGetOutboxListQuery();
            var outBoxList = await sender.Send(request, cancellationToken);
            if (outBoxList.Count <= 0) continue;

            var type = GetOutBoxType(outBoxList);
            if (type == null)
                throw new Exception("type is null");

            var items = outBoxList.Items.Select(x => x.Content).ToList();
            var deserializedContent = items.Select(content => JsonSerializer.Deserialize(content, type)).ToList();
            if (!deserializedContent.Any())
                throw new ApplicationException($"No content found for type {type}");

            var events = deserializedContent.Cast<IEvent>().ToList();
            eventBus.Publish(events);


            var updateProcessAtCommand = outBoxList.ToUpdateProcessAtCommand();
            await sender.Send(updateProcessAtCommand, cancellationToken);
        }

        await Task.Delay(1000, cancellationToken);
    }

    private static Type? GetOutBoxType(GetListResponse<GetOutboxListDto> outBoxList)
    {
        return AppDomain.CurrentDomain
            .GetAssemblies()
            .Select(assembly => assembly.GetType(outBoxList.Items.First().Type))
            .FirstOrDefault(t => t != null);
    }
}