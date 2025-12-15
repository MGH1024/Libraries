using Library.Domain.Libraries.Events;
using MGH.Core.Infrastructure.EventBus;

namespace Library.Endpoint.Worker.Inbox.EventHandlers;

public class LibraryCreatedDomainEventHandler : IEventHandler<LibraryAddedDomainEvent>
{
    private readonly ILogger<LibraryCreatedDomainEventHandler> _logger;

    public LibraryCreatedDomainEventHandler(ILogger<LibraryCreatedDomainEventHandler> logger)
    {
        _logger = logger;
    }

    public Task HandleAsync(LibraryAddedDomainEvent message)
    {
        _logger.LogInformation("Received LibraryCreatedDomainEvent message: {Name}", message.Id);
        // Logic here
        return Task.CompletedTask;
    }
}