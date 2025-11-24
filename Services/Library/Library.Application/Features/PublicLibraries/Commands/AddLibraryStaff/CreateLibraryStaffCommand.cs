using MediatR;
using Library.Domain;
using MGH.Core.Application.Buses.Commands;
using Library.Domain.Libraries.ValueObjects;
using MGH.Core.Application.Pipelines.Authorization;
using Library.Application.Features.PublicLibraries.Profiles;
using Library.Application.Features.PublicLibraries.Constants;
using Library.Application.Features.PublicLibraries.Rules;

namespace Library.Application.Features.PublicLibraries.Commands.AddLibraryStaff;

[Roles(PublicLibraryOperationClaims.AddStaff)]
public class CreateLibraryStaffCommand : ICommand<Unit>
{
    public string Name { get; set; }
    public string Position { get; set; }
    public string NationalCode { get; set; }
    public Guid LibraryId { get; set; }
}

public class AddLibraryStaffCommandHandler(LibraryBusinessRules libraryBusinessRules, IUow uow)
    : ICommandHandler<CreateLibraryStaffCommand, Unit>
{
    public async Task<Unit> Handle(CreateLibraryStaffCommand request, CancellationToken cancellationToken)
    {
        var library = await uow.Library.GetAsync(request.ToGetBaseLibraryModel());
        await libraryBusinessRules.LibraryShouldBeExistsWhenSelected(library);
        library.AddLibraryStaff(new Staff(request.Name, request.Position, request.NationalCode));
        await uow.CompleteAsync(cancellationToken);
        return Unit.Value;
    }
}