using Library.Domain.Libraries.Events;
using MGH.Core.Infrastructure.EventBus;

namespace Library.Endpoint.Worker.Inbox.ConsumerHandlers;

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


//payload message :
//{
//  "libraryName": "Test",
//  "libraryCode": "101",
//  "libraryLocation": "location",
//  "libraryDistrict": 1,
//  "libraryRegistrationDate": "2025-07-26T12:00:00Z"
//}

//library.created.routing.key