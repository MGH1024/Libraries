using Library.Domain.Libraries.Events;
using MGH.Core.Infrastructure.EventBus;

namespace Library.Endpoint.Worker.Inbox.EventHandlers;

public class LibraryCreatedDomainEvent2Handler : IEventHandler<LibraryCreatedDomainEvent2>
{
    private readonly ILogger<LibraryCreatedDomainEvent2Handler> _logger;

    public LibraryCreatedDomainEvent2Handler(ILogger<LibraryCreatedDomainEvent2Handler> logger)
    {
        _logger = logger;
    }

    public Task HandleAsync(LibraryCreatedDomainEvent2 message)
    {
        _logger.LogInformation("Received LibraryCreatedDomainEvent2 message: {Name}", message.LibraryName);
        // Logic here
        return Task.CompletedTask;
    }
}