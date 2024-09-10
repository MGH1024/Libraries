using Application.Features.OperationClaims.Constants;
using Application.Features.OperationClaims.Rules;
using AutoMapper;
using Domain;
using MGH.Core.Application.Pipelines.Authorization;
using MGH.Core.Domain.Buses.Commands;
using MGH.Core.Infrastructure.Securities.Security.Entities;
using MGH.Core.Persistence.Models.Filters.GetModels;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.OperationClaims.Commands.Delete;

[Roles(OperationClaimOperationClaims.DeleteOperationClaims)]
public class DeleteOperationClaimCommand : ICommand<DeletedOperationClaimResponse>
{
    public int Id { get; set; }
}

public class DeleteOperationClaimCommandHandler(
    IUow uow,
    IMapper mapper,
    OperationClaimBusinessRules operationClaimBusinessRules) : ICommandHandler<DeleteOperationClaimCommand, DeletedOperationClaimResponse>
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
        await uow.OperationClaim.DeleteAsync(entity: operationClaim!, cancellationToken: cancellationToken);

        return mapper.Map<DeletedOperationClaimResponse>(operationClaim);
    }
}