using Application.Features.OutBoxes.Queries.GetList;
using MediatR;
using MGH.Core.Application.Requests;
using Application.Features.Libraries.Extensions;

namespace Library.Worker.Outbox;

public class Worker(ILogger<Worker> logger, ISender sender) : BackgroundService
{
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var getOutboxListQuery = 
                new GetOutboxListQuery (new PageRequest{PageIndex = 0,PageSize = 1000 });
            
            var result = await sender.Send(getOutboxListQuery,stoppingToken);
            
            if (logger.IsEnabled(LogLevel.Information))
            {
                logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            }

            await Task.Delay(1000, stoppingToken);
        }
    }
}