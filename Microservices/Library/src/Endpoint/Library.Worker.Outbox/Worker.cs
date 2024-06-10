using Application.Features.OutBoxes.Queries.GetList;
using MediatR;
using MGH.Core.Application.Requests;
using Application.Features.Libraries.Extensions;
using Domain.Entities.Libraries;
using MGH.Core.Infrastructure.Public;

namespace Library.Worker.Outbox;

public class Worker(ILogger<Worker> logger, ISender sender, IOutBoxRepository outBoxRepository, IDateTime dateTime)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var getOutboxListQuery =
                new GetOutboxListQuery(new PageRequest { PageIndex = 0, PageSize = 1000 });

            var result = await sender.Send(getOutboxListQuery, stoppingToken);
            foreach (var getOutboxListDto in result.Items)
            {
                var outbox = await outBoxRepository.GetAsync(a => a.Id == getOutboxListDto.Id,
                    cancellationToken: stoppingToken);

                outbox.ProcessedAt = dateTime.IranNow;
                outBoxRepository.Update(outbox);
                //publish
            }

            if (logger.IsEnabled(LogLevel.Information))
            {
                logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            }

            await Task.Delay(1000, stoppingToken);
        }
    }
}