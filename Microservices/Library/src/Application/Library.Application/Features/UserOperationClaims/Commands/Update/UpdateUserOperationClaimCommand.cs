using Application.Features.UserOperationClaims.Constants;
using Application.Features.UserOperationClaims.Rules;
using AutoMapper;
using Domain;
using MGH.Core.Application.Pipelines.Authorization;
using MGH.Core.Domain.Buses.Commands;
using MGH.Core.Infrastructure.Securities.Security.Entities;
using MGH.Core.Persistence.Models.Filters.GetModels;
using static Application.Features.UserOperationClaims.Constants.UserOperationClaimsOperationClaims;

namespace Application.Features.UserOperationClaims.Commands.Update;

public class UpdateUserOperationClaimCommand : ICommand<UpdatedUserOperationClaimResponse>, ISecuredRequest
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int OperationClaimId { get; set; }

    public string[] Roles => new[] { Admin, Write, UserOperationClaimsOperationClaims.Update };

    public class UpdateUserOperationClaimCommandHandler(
        IUow uow,
        IMapper mapper,
        UserOperationClaimBusinessRules userOperationClaimBusinessRules)
        : ICommandHandler<UpdateUserOperationClaimCommand, UpdatedUserOperationClaimResponse>
    {
        public async Task<UpdatedUserOperationClaimResponse> Handle(
            UpdateUserOperationClaimCommand request,
            CancellationToken cancellationToken
        )
        {
            var userOperationClaim = await uow.UserOperationClaim.GetAsync(
                new GetModel<UserOperationClaim>
                {
                    Predicate = uoc => uoc.Id == request.Id,
                    EnableTracking = false,
                    CancellationToken = cancellationToken
                });
            await userOperationClaimBusinessRules.UserOperationClaimShouldExistWhenSelected(userOperationClaim);
            await userOperationClaimBusinessRules.UserShouldNotHasOperationClaimAlreadyWhenUpdated(
                request.Id,
                request.UserId,
                request.OperationClaimId
            );
            var mappedUserOperationClaim = mapper.Map(request, destination: userOperationClaim!);
            var updatedUserOperationClaim =
                await uow.UserOperationClaim.UpdateAsync(mappedUserOperationClaim, cancellationToken);
            var updatedUserOperationClaimDto = mapper.Map<UpdatedUserOperationClaimResponse>(updatedUserOperationClaim);
            return updatedUserOperationClaimDto;
        }
    }
}