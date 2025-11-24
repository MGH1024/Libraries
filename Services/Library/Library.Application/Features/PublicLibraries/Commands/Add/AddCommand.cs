using Library.Domain.Libraries.Constant;
using MGH.Core.Application.Buses.Commands;
using MGH.Core.Application.Pipelines.Authorization;
using Library.Application.Features.PublicLibraries.Constants;

namespace Library.Application.Features.PublicLibraries.Commands.Add;

[Roles(PublicLibraryOperationClaims.Add)]
public class AddCommand : ICommand<Guid>
{
    public string Name { get; set; }
    public string Code { get; set; }
    public string Location { get; set; }
    public DistrictEnum District { get; set; }
    public DateTime RegistrationTime { get; set; }
}
