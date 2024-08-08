using Application.Features.Libraries.Extensions;
using Application.Features.Libraries.Rules;
using Domain.Entities.Libraries;
using Domain.Entities.Libraries.ValueObjects;
using MediatR;
using MGH.Core.Domain.Buses.Commands;
using MGH.Core.Infrastructure.Persistence.Persistence.Base;
using MGH.Core.Infrastructure.Persistence.Persistence.Models.Filters;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Libraries.Commands.AddLibraryStaff;

public class CreateLibraryStaffCommand : ICommand<Unit>
{
    public string Name { get; set; }
    public string Position { get; set; }
    public string NationalCode { get; set; }
    public Guid LibraryId { get; set; }
}

public class AddLibraryStaffCommandHandler(
    ILibraryRepository libraryRepository,
    LibraryBusinessRules libraryBusinessRules,
    IUnitOfWork unitOfWork)
    : ICommandHandler<CreateLibraryStaffCommand, Unit>
{
    public async Task<Unit> Handle(CreateLibraryStaffCommand request, CancellationToken cancellationToken)
    {
        var library = await libraryRepository.GetAsync(request.ToGetBaseLibraryModel(cancellationToken));
        await libraryBusinessRules.LibraryShouldBeExistsWhenSelected(library);
        library.AddLibraryStaff(new Staff(request.Name, request.Position, request.NationalCode));
        await unitOfWork.CompleteAsync(cancellationToken);
        return Unit.Value;
    }
}