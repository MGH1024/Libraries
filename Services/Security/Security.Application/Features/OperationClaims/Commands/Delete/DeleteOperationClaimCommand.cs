using Application.Features.OperationClaims.Constants;
using MGH.Core.Application.Pipelines.Authorization;
using MGH.Core.Domain.Buses.Commands;

namespace Application.Features.OperationClaims.Commands.Delete;

[Roles(OperationClaimOperationClaims.DeleteOperationClaims)]
public class DeleteOperationClaimCommand : ICommand<DeletedOperationClaimResponse>
{
    public int Id { get; set; }
}