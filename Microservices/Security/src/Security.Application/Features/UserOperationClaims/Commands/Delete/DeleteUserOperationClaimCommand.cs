using Application.Features.UserOperationClaims.Constants;
using Application.Features.UserOperationClaims.Rules;
using AutoMapper;
using Domain;
using MGH.Core.Application.Pipelines.Authorization;
using MGH.Core.Domain.Buses.Commands;
using MGH.Core.Infrastructure.Securities.Security.Entities;
using MGH.Core.Persistence.Models.Filters.GetModels;

namespace Application.Features.UserOperationClaims.Commands.Delete;

[Roles(UserOperationClaimOperationClaims.DeleteUserOperationClaims)]
public class DeleteUserOperationClaimCommand : ICommand<DeletedUserOperationClaimResponse>
{
    public int Id { get; set; }
}

public class DeleteUserOperationClaimCommandHandler(
    IUow uow,
    IMapper mapper,
    UserOperationClaimBusinessRules userOperationClaimBusinessRules)
    : ICommandHandler<DeleteUserOperationClaimCommand, DeletedUserOperationClaimResponse>
{
    public async Task<DeletedUserOperationClaimResponse> Handle(DeleteUserOperationClaimCommand request, CancellationToken cancellationToken)
    {
        var getUserModel = mapper.Map<GetModel<UserOperationClaim>>(request, opt => opt.Items["CancellationToken"] = cancellationToken);
        var userOperationClaim = await uow.UserOperationClaim.GetAsync(getUserModel);
        await userOperationClaimBusinessRules.UserOperationClaimShouldExistWhenSelected(userOperationClaim);
        await uow.UserOperationClaim.DeleteAsync(userOperationClaim!);
        return mapper.Map<DeletedUserOperationClaimResponse>(userOperationClaim);
    }
}