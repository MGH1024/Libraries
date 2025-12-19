using Library.Domain.Libraries.Events;
using MGH.Core.Infrastructure.EventBus;

namespace Library.Endpoint.Worker.Inbox.EventHandlers;

public class LibraryAddedEventHandler : IEventHandler<LibraryAdded>
{
    private readonly ILogger<LibraryAddedEventHandler> _logger;

    public LibraryAddedEventHandler(ILogger<LibraryAddedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task HandleAsync(LibraryAdded message)
    {
        _logger.LogInformation("Received LibraryCreatedDomainEvent message: {Name}", message.Id);
        // Logic here
        return Task.CompletedTask;
    }
}