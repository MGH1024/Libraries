using Application.Features.OperationClaims.Constants;
using Application.Features.OperationClaims.Rules;
using AutoMapper;
using Domain;
using MGH.Core.Application.Pipelines.Authorization;
using MGH.Core.Domain.Buses.Commands;
using MGH.Core.Infrastructure.Securities.Security.Entities;
using MGH.Core.Persistence.Models.Filters.GetModels;

namespace Application.Features.OperationClaims.Commands.Update;

[Roles(OperationClaimOperationClaims.UpdateOperationClaims)]
public class UpdateOperationClaimCommand(int id, string name) : ICommand<UpdatedOperationClaimResponse>
{
    public int Id { get; set; } = id;
    public string Name { get; set; } = name;

    public UpdateOperationClaimCommand() : this(0, string.Empty)
    {
    }
}

public class UpdateOperationClaimCommandHandler(
    IUow uow,
    IMapper mapper,
    OperationClaimBusinessRules operationClaimBusinessRules)
    : ICommandHandler<UpdateOperationClaimCommand, UpdatedOperationClaimResponse>
{
    public async Task<UpdatedOperationClaimResponse> Handle(UpdateOperationClaimCommand request,
        CancellationToken cancellationToken)
    {
        var operationClaim = await uow.OperationClaim.GetAsync(
            new GetModel<OperationClaim>
            {
                Predicate = oc => oc.Id == request.Id,
                CancellationToken = cancellationToken
            });
        await operationClaimBusinessRules.OperationClaimShouldExistWhenSelected(operationClaim);
        await operationClaimBusinessRules.OperationClaimNameShouldNotExistWhenUpdating(request.Id, request.Name,
            cancellationToken);
        var mappedOperationClaim = mapper.Map(request, destination: operationClaim!);

        var updatedOperationClaim =
            await uow.OperationClaim.UpdateAsync(mappedOperationClaim, cancellationToken);

        return mapper.Map<UpdatedOperationClaimResponse>(updatedOperationClaim);
    }
}