using AutoMapper;
using MGH.Core.Domain.Buses.Commands;
using MGH.Core.Infrastructure.Persistence.Models.Filters.GetModels;
using MGH.Core.Infrastructure.Securities.Security.Entities;
using Microsoft.EntityFrameworkCore;
using Security.Application.Features.OperationClaims.Rules;
using Security.Domain;

namespace Security.Application.Features.OperationClaims.Commands.Delete;

public class DeleteOperationClaimCommandHandler(
    IUow uow,
    IMapper mapper,
    IOperationClaimBusinessRules operationClaimBusinessRules) : ICommandHandler<DeleteOperationClaimCommand, DeletedOperationClaimResponse>
{
    public async Task<DeletedOperationClaimResponse> Handle(DeleteOperationClaimCommand request, CancellationToken cancellationToken)
    {
        var operationClaim = await uow.OperationClaim.GetAsync(
            new GetModel<OperationClaim>
            {
                Predicate = oc => oc.Id == request.Id,
                Include = q => q.Include(oc => oc.UserOperationClaims),
                CancellationToken = cancellationToken
            });

        await operationClaimBusinessRules.OperationClaimShouldExistWhenSelected(operationClaim);
        await uow.OperationClaim.DeleteAsync(entity: operationClaim!);

        return mapper.Map<DeletedOperationClaimResponse>(operationClaim);
    }
}