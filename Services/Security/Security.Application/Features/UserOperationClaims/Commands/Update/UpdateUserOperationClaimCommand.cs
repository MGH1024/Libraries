using AutoMapper;
using MGH.Core.Application.Pipelines.Authorization;
using MGH.Core.Domain.Buses.Commands;
using MGH.Core.Infrastructure.Persistence.Models.Filters.GetModels;
using MGH.Core.Infrastructure.Securities.Security.Entities;
using Security.Application.Features.UserOperationClaims.Constants;
using Security.Application.Features.UserOperationClaims.Rules;
using Security.Domain;

namespace Security.Application.Features.UserOperationClaims.Commands.Update;

[Roles(UserOperationClaimOperationClaims.UpdateUserOperationClaims)]
public class UpdateUserOperationClaimCommand : ICommand<UpdatedUserOperationClaimResponse>
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int OperationClaimId { get; set; }
}

public class UpdateUserOperationClaimCommandHandler(
    IUow uow,
    IMapper mapper,
    IUserOperationClaimBusinessRules userOperationClaimBusinessRules)
    : ICommandHandler<UpdateUserOperationClaimCommand, UpdatedUserOperationClaimResponse>
{
    public async Task<UpdatedUserOperationClaimResponse> Handle(UpdateUserOperationClaimCommand request, CancellationToken cancellationToken)
    {
        var getUserModel = mapper.Map<GetModel<UserOperationClaim>>(request, opt => opt.Items["CancellationToken"] = cancellationToken);
        var userOperationClaim = await uow.UserOperationClaim.GetAsync(getUserModel);
        await userOperationClaimBusinessRules.UserOperationClaimShouldExistWhenSelected(userOperationClaim);
        await userOperationClaimBusinessRules.UserShouldNotHasOperationClaimAlreadyWhenUpdated(
            request.Id,
            request.UserId,
            request.OperationClaimId,
            cancellationToken
        );
        var mappedUserOperationClaim = mapper.Map(request, destination: userOperationClaim!);
        var updatedUserOperationClaim = await uow.UserOperationClaim.UpdateAsync(mappedUserOperationClaim, cancellationToken);
        return mapper.Map<UpdatedUserOperationClaimResponse>(updatedUserOperationClaim);
    }
}