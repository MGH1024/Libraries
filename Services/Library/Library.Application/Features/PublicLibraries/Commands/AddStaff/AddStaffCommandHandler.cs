using MediatR;
using Library.Domain;
using MGH.Core.Application.Buses;
using Library.Domain.Libraries.ValueObjects;
using Library.Application.Features.PublicLibraries.Rules;
using Library.Application.Features.PublicLibraries.Profiles;

namespace Library.Application.Features.PublicLibraries.Commands.AddStaff;

public class AddStaffCommandHandler(LibraryBusinessRules libraryBusinessRules, IUow uow)
    : ICommandHandler<AddStaffCommand, Unit>
{
    public async Task<Unit> Handle(AddStaffCommand request, CancellationToken cancellationToken)
    {
        var library = await uow.Library.GetAsync(request.ToGetBaseLibraryModel());
        await libraryBusinessRules.LibraryShouldBeExistsWhenSelected(library);
        library.AddLibraryStaff(new Staff(request.Name, request.Position, request.NationalCode));
        await uow.CompleteAsync(cancellationToken);
        return Unit.Value;
    }
}