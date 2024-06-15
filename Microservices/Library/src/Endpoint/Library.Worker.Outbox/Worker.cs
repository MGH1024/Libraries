using MediatR;
using MGH.Core.Application.Requests;
using Application.Features.OutBoxes.Queries.GetList;

namespace Library.Worker.Outbox;

public class Worker : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    public Worker(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var sender = scope.ServiceProvider.GetRequiredService<ISender>();
        
        while (!cancellationToken.IsCancellationRequested)
        {
            var result = await sender.Send(
                new GetOutboxListQuery(new PageRequest { PageIndex = 0, PageSize = 1000 }), cancellationToken);

            await Task.Delay(1000, cancellationToken);
        }
    }
}