using Application.Features.OperationClaims.Rules;
using AutoMapper;
using Domain.Security;
using MGH.Core.Application.Pipelines.Authorization;
using MGH.Core.Domain.Buses.Commands;
using MGH.Core.Infrastructure.Securities.Security.Entities;
using static Application.Features.OperationClaims.Constants.OperationClaimsOperationClaims;

namespace Application.Features.OperationClaims.Commands.Create;

public class CreateOperationClaimCommand(string name) : ICommand<CreatedOperationClaimResponse>, ISecuredRequest
{
    public string Name { get; set; } = name;

    public CreateOperationClaimCommand() : this(string.Empty)
    {
    }

    public string[] Roles => new[] { Admin, Write, Add };

    public class CreateOperationClaimCommandHandler(
        IOperationClaimRepository operationClaimRepository,
        IMapper mapper,
        OperationClaimBusinessRules operationClaimBusinessRules)
        : ICommandHandler<CreateOperationClaimCommand, CreatedOperationClaimResponse>
    {
        public async Task<CreatedOperationClaimResponse> Handle(CreateOperationClaimCommand request, CancellationToken cancellationToken)
        {
            await operationClaimBusinessRules.OperationClaimNameShouldNotExistWhenCreating(request.Name,cancellationToken);
            OperationClaim mappedOperationClaim = mapper.Map<OperationClaim>(request);

            OperationClaim createdOperationClaim = await operationClaimRepository.AddAsync(mappedOperationClaim,cancellationToken);

            CreatedOperationClaimResponse response = mapper.Map<CreatedOperationClaimResponse>(createdOperationClaim);
            return response;
        }
    }
}
