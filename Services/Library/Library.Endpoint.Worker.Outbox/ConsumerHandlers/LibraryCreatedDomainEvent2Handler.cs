using Library.Domain.Libraries.Events;
using MGH.Core.Infrastructure.EventBus;

namespace Library.Endpoint.Worker.Outbox.ConsumerHandlers;

public class LibraryCreatedDomainEvent2Handler(ILogger<LibraryCreatedDomainEvent2Handler> logger) : IEventHandler<LibraryCreatedDomainEvent2>
{
    public Task HandleAsync(LibraryCreatedDomainEvent2 message)
    {
        logger.LogInformation("Received LibraryCreatedDomainEvent2 message");
        //logic
        return Task.CompletedTask;
    }
}