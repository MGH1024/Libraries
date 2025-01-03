using AutoMapper;
using MGH.Core.Application.Pipelines.Authorization;
using MGH.Core.Domain.Buses.Commands;
using MGH.Core.Infrastructure.Persistence.Models.Filters.GetModels;
using MGH.Core.Infrastructure.Securities.Security.Entities;
using Security.Application.Features.OperationClaims.Constants;
using Security.Application.Features.OperationClaims.Rules;
using Security.Domain;

namespace Security.Application.Features.OperationClaims.Commands.Update;

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
    IOperationClaimBusinessRules operationClaimBusinessRules)
    : ICommandHandler<UpdateOperationClaimCommand, UpdatedOperationClaimResponse>
{
    public async Task<UpdatedOperationClaimResponse> Handle(UpdateOperationClaimCommand request, CancellationToken cancellationToken)
    {
        var getUserModel = mapper.Map<GetModel<OperationClaim>>(request, opt => opt.Items["CancellationToken"] = cancellationToken);
        var operationClaim = await uow.OperationClaim.GetAsync(getUserModel);
        await operationClaimBusinessRules.OperationClaimShouldExistWhenSelected(operationClaim);

        await operationClaimBusinessRules.OperationClaimNameShouldNotExistWhenUpdating(request.Id, request.Name, cancellationToken);
        var mappedOperationClaim = mapper.Map(request, destination: operationClaim!);

        var updatedOperationClaim = await uow.OperationClaim.UpdateAsync(mappedOperationClaim, cancellationToken);
        return mapper.Map<UpdatedOperationClaimResponse>(updatedOperationClaim);
    }
}