using MGH.Core.Application.Pipelines.Authorization;
using MGH.Core.Domain.Buses.Commands;
using Security.Application.Features.OperationClaims.Constants;

namespace Security.Application.Features.OperationClaims.Commands.Delete;

[Roles(OperationClaimOperationClaims.DeleteOperationClaims)]
public class DeleteOperationClaimCommand : ICommand<DeletedOperationClaimResponse>
{
    public int Id { get; set; }
}