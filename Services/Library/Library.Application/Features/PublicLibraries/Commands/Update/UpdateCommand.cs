using Library.Domain.Libraries.Constant;
using MGH.Core.Application.Buses.Commands;
using MGH.Core.Application.Pipelines.Authorization;
using Library.Application.Features.PublicLibraries.Constants;

namespace Library.Application.Features.PublicLibraries.Commands.Update;

[Roles(PublicLibraryOperationClaims.Update)]
public class UpdateCommand : ICommand<Guid>
{
    public Guid LibraryId { get; set; }
    public required string Name { get; set; }
    public required string Code { get; set; }
    public required string Location { get; set; }
    public DistrictEnum DistrictEnum { get; set; }
    public DateTime RegistrationDate { get; set; }
}
