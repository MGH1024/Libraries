using Application.Features.OperationClaims.Constants;
using Application.Features.OperationClaims.Rules;
using AutoMapper;
using Domain.Security;
using MGH.Core.Application.Pipelines.Authorization;
using MGH.Core.Domain.Buses.Commands;
using MGH.Core.Infrastructure.Securities.Security.Entities;
using MGH.Core.Persistence.Models.Filters.GetModels;
using static Application.Features.OperationClaims.Constants.OperationClaimsOperationClaims;

namespace Application.Features.OperationClaims.Commands.Update;

public class UpdateOperationClaimCommand(int id, string name) : ICommand<UpdatedOperationClaimResponse>, ISecuredRequest
{
    public int Id { get; set; } = id;
    public string Name { get; set; } = name;

    public UpdateOperationClaimCommand() : this(0, string.Empty)
    {
    }

    public string[] Roles => new[] { Admin, Write, OperationClaimsOperationClaims.Update };

    public class
        UpdateOperationClaimCommandHandler : ICommandHandler<UpdateOperationClaimCommand, UpdatedOperationClaimResponse>
    {
        private readonly IOperationClaimRepository _operationClaimRepository;
        private readonly IMapper _mapper;
        private readonly OperationClaimBusinessRules _operationClaimBusinessRules;

        public UpdateOperationClaimCommandHandler(
            IOperationClaimRepository operationClaimRepository,
            IMapper mapper,
            OperationClaimBusinessRules operationClaimBusinessRules
        )
        {
            _operationClaimRepository = operationClaimRepository;
            _mapper = mapper;
            _operationClaimBusinessRules = operationClaimBusinessRules;
        }

        public async Task<UpdatedOperationClaimResponse> Handle(UpdateOperationClaimCommand request,
            CancellationToken cancellationToken)
        {
            var operationClaim = await _operationClaimRepository.GetAsync(
                new GetModel<OperationClaim>
                {
                    Predicate = oc => oc.Id == request.Id,
                    CancellationToken = cancellationToken
                });
            await _operationClaimBusinessRules.OperationClaimShouldExistWhenSelected(operationClaim);
            await _operationClaimBusinessRules.OperationClaimNameShouldNotExistWhenUpdating(request.Id, request.Name,
                cancellationToken);
            var mappedOperationClaim = _mapper.Map(request, destination: operationClaim!);

            var updatedOperationClaim =
                await _operationClaimRepository.UpdateAsync(mappedOperationClaim, cancellationToken);

            var response = _mapper.Map<UpdatedOperationClaimResponse>(updatedOperationClaim);
            return response;
        }
    }
}