using MediatR;
using Application.Features.OutBoxes.Commands.UpdateProcessAt;
using MGH.Core.Application.Requests;
using Application.Features.OutBoxes.Queries.GetList;
using MGH.Core.Infrastructure.ElasticSearch.ElasticSearch.Base;
using MGH.Core.Infrastructure.MessageBroker;

namespace Library.Worker.Outbox;

public class Worker(IServiceProvider serviceProvider) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        using var scope = serviceProvider.CreateScope();
        var sender = scope.ServiceProvider.GetRequiredService<ISender>();
        var elastic = scope.ServiceProvider.GetRequiredService<IElasticSearch>();
        var messageBus = scope.ServiceProvider
            .GetRequiredService<IMessageSender<GetOutboxListDto>>();

        while (!cancellationToken.IsCancellationRequested)
        {
            //using var transactionScope = new TransactionScope();
            var result = await sender.Send(
                new GetOutboxListQuery(new PageRequest
                {
                    PageIndex = 0,
                    PageSize = 1000
                }), cancellationToken);

            await sender.Send(new UpdateProcessAtCommand
            {
                Guids = result.Items.Select(a => a.Id)
            }, cancellationToken);

            await elastic.InsertManyAsync("libraries", result.Items.ToArray());
            messageBus.Publish(new BatchMessageModel<GetOutboxListDto>()
            {
                Items = result.Items.ToList(),
                BaseMessage = new BaseMessage
                {
                    ExchangeName = "mgh-exchange",
                    RoutingKey = "mgh-routingkey",
                    QueueName = "mgh-queue",
                    ExchangeType = "direct",
                }
            });

            //transactionScope.Complete();
        }

        await Task.Delay(1000, cancellationToken);
    }
}