using Library.Domain;
using MGH.Core.Application.Buses;
using Library.Domain.Libraries.Exceptions;
using Library.Domain.Libraries.Factories;

namespace Library.Application.Features.PublicLibraries.Commands.Update;

public class UpdateCommandHandler(
    IUow uow,
    IPublicLibraryFactory factory)
    : ICommandHandler<UpdateCommand, Guid>
{
    public async Task<Guid> Handle(
        UpdateCommand request,
        CancellationToken cancellationToken)
    {
        var library = await uow.Library.GetAsync(request.Id);
        if (library is null)
            throw new LibraryNotFoundException();

        factory.Update(
            library,
            request.Name,
            request.Location,
            request.District,
            request.RegistrationTime);
        await uow.CompleteAsync(cancellationToken);
        return library.Id;
    }
}