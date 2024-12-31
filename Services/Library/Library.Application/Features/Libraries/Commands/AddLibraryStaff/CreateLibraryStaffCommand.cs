using Library.Application.Features.Libraries.Constants;
using Library.Application.Features.Libraries.Profiles;
using Library.Application.Features.Libraries.Rules;
using Library.Domain;
using Library.Domain.Libraries.ValueObjects;
using MediatR;
using MGH.Core.Application.Pipelines.Authorization;
using MGH.Core.Domain.Buses.Commands;

namespace Library.Application.Features.Libraries.Commands.AddLibraryStaff;

[Roles(LibraryOperationClaims.AddStaff)]
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
        var library = await uow.Library.GetAsync(request.ToGetBaseLibraryModel(cancellationToken));
        await libraryBusinessRules.LibraryShouldBeExistsWhenSelected(library);
        library.AddLibraryStaff(new Staff(request.Name, request.Position, request.NationalCode));
        await uow.CompleteAsync(cancellationToken);
        return Unit.Value;
    }
}