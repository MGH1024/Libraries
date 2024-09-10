using Application.Features.UserOperationClaims.Constants;
using Application.Features.UserOperationClaims.Rules;
using AutoMapper;
using Domain;
using MGH.Core.Application.Pipelines.Authorization;
using MGH.Core.Domain.Buses.Commands;
using MGH.Core.Infrastructure.Securities.Security.Entities;

namespace Application.Features.UserOperationClaims.Commands.Create;

[Roles(UserOperationClaimOperationClaims.AddUserOperationClaims)]
public class CreateUserOperationClaimCommand : ICommand<CreatedUserOperationClaimResponse>
{
    public int UserId { get; set; }
    public int OperationClaimId { get; set; }
}

public class CreateUserOperationClaimCommandHandler(
    IUow uow,
    IMapper mapper,
    UserOperationClaimBusinessRules userOperationClaimBusinessRules)
    : ICommandHandler<CreateUserOperationClaimCommand, CreatedUserOperationClaimResponse>
{
    public async Task<CreatedUserOperationClaimResponse> Handle(
        CreateUserOperationClaimCommand request,
        CancellationToken cancellationToken
    )
    {
        await userOperationClaimBusinessRules.UserShouldNotHasOperationClaimAlreadyWhenInsert(
            request.UserId,
            request.OperationClaimId
        );
        var mappedUserOperationClaim = mapper.Map<UserOperationClaim>(request);

        var createdUserOperationClaim =
            await uow.UserOperationClaim.AddAsync(mappedUserOperationClaim, cancellationToken);

        var createdUserOperationClaimDto =
            mapper.Map<CreatedUserOperationClaimResponse>(
                createdUserOperationClaim
            );
        return createdUserOperationClaimDto;
    }
}