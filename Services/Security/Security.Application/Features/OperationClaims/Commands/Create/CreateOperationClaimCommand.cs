﻿using Domain;
using AutoMapper;
using MGH.Core.Domain.Buses.Commands;
using Application.Features.OperationClaims.Rules;
using Application.Features.OperationClaims.Constants;
using MGH.Core.Application.Pipelines.Authorization;
using MGH.Core.Infrastructure.Securities.Security.Entities;

namespace Application.Features.OperationClaims.Commands.Create;

[Roles(OperationClaimOperationClaims.AddOperationClaims)]
public class CreateOperationClaimCommand(string name) : ICommand<CreatedOperationClaimResponse>
{
    public string Name { get; set; } = name;

    public CreateOperationClaimCommand() : this(string.Empty)
    {
    }
}

public class CreateOperationClaimCommandHandler(IUow uow , IMapper mapper, OperationClaimBusinessRules
    operationClaimBusinessRules) : ICommandHandler<CreateOperationClaimCommand, CreatedOperationClaimResponse>
{
    public async Task<CreatedOperationClaimResponse> Handle(CreateOperationClaimCommand request, CancellationToken cancellationToken)
    {
        await operationClaimBusinessRules.OperationClaimNameShouldNotExistWhenCreating(request.Name, cancellationToken);
        var mappedOperationClaim = mapper.Map<OperationClaim>(request);
        var createdOperationClaim = await uow.OperationClaim.AddAsync(mappedOperationClaim, cancellationToken);
        return mapper.Map<CreatedOperationClaimResponse>(createdOperationClaim);
    }
}