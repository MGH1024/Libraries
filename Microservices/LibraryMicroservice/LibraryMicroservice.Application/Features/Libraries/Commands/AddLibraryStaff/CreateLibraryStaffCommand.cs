using Application.Features.Libraries.Rules;
using Domain.Entities.Libraries;
using Domain.Entities.Libraries.ValueObjects;
using MediatR;
using MGH.Core.Application.Buses.Commands;
using MGH.Core.Persistence.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Libraries.Commands.AddLibraryStaff;

public class CreateLibraryStaffCommand : ICommand<Unit>
{
    public string Name { get; set; }
    public string Position { get; set; }
    public string NationalCode { get; set; }
    public Guid LibraryId { get; set; }
}

public class AddLibraryStaffCommandHandler(ILibraryRepository libraryRepository,
    LibraryBusinessRules libraryBusinessRules,
    IUnitOfWork unitOfWork)
    : ICommandHandler<CreateLibraryStaffCommand, Unit>
{
    public async Task<Unit> Handle(CreateLibraryStaffCommand request, CancellationToken cancellationToken)
    {
        var library = await libraryRepository
            .GetAsync(a => a.Id == request.LibraryId,
                a => a.Include(b => b.LibraryStaves),
                cancellationToken: cancellationToken);

        await libraryBusinessRules.LibraryShouldBeExistsWhenSelected(library);

        library.AddLibraryStaff(new LibraryStaff(request.Name, request.Position, request.NationalCode));
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}