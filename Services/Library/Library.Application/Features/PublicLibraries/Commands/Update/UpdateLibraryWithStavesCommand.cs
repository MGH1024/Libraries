using Library.Application.Features.PublicLibraries.Constants;
using Library.Application.Features.PublicLibraries.Rules;
using Library.Domain.Libraries.Constant;
using Library.Domain.Libraries.ValueObjects;
using MGH.Core.Application.Buses;
using MGH.Core.Application.Pipelines.Authorization;
using MGH.Core.CrossCutting.Exceptions.Types;

namespace Library.Application.Features.PublicLibraries.Commands.Update;

[Roles(PublicLibraryOperationClaims.UpdateWithStaves)]
public class UpdateLibraryWithStavesCommand : ICommand<Guid>
{
    public Guid LibraryId { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
    public string Location { get; set; }
    public DistrictEnum DistrictEnum { get; set; }
    public DateTime RegistrationDate { get; set; }
    public List<StaffDto> StavesDto { get; set; }
}
