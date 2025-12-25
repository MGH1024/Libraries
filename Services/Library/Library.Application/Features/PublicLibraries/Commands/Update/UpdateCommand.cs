using MGH.Core.Application.Buses;
using Library.Domain.Libraries.Constant;
using MGH.Core.Application.Pipelines.Authorization;
using Library.Application.Features.PublicLibraries.Constants;

namespace Library.Application.Features.PublicLibraries.Commands.Update;

[Roles(PublicLibraryOperationClaims.Update)]
public class UpdateCommand : ICommand<UpdateCommandResponse>
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Location { get; set; }
    public DistrictEnum District { get; set; }
    public DateTime RegistrationTime { get; set; }
}
