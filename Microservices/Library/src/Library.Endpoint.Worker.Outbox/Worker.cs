using MediatR;
using MGH.Core.Application.Requests;
using Application.Features.OutBoxes.Queries.GetList;
using MGH.Core.Infrastructure.MessageBroker.RabbitMq;
using Application.Features.OutBoxes.Commands.UpdateProcessAt;
using MGH.Core.Infrastructure.ElasticSearch.ElasticSearch.Base;
using MGH.Core.Infrastructure.MessageBroker.RabbitMq.Atributes;

namespace Library.Endpoint.Worker.Outbox;

[BaseMessage("mgh-routingkey", "direct", "mgh-exchange", "mgh-queue")]
public class Worker(IServiceProvider serviceProvider) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        using var scope = serviceProvider.CreateScope();
        var sender = scope.ServiceProvider.GetRequiredService<ISender>();
        var elastic = scope.ServiceProvider.GetRequiredService<IElasticSearch>();

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

            // ReSharper disable once CoVariantArrayConversion
            await elastic.InsertManyAsync("libraries", result.Items.ToArray());

            

            //transactionScope.Complete();
        }

        await Task.Delay(1000, cancellationToken);
    }
}