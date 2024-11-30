﻿using Domain;
using AutoMapper;
using MGH.Core.Infrastructure.Public;
using MGH.Core.Domain.Buses.Commands;
using Application.Features.Auth.Rules;
using Application.Features.Auth.Services;
using MGH.Core.Infrastructure.Securities.Security.Entities;
using MGH.Core.Infrastructure.Persistence.EF.Models.Filters.GetModels;

namespace Application.Features.Auth.Commands.RevokeToken;

public class RevokeTokenCommandHandler(
    IAuthService authService,
    IAuthBusinessRules authBusinessRules,
    IUow uow,
    IDateTime time,
    IMapper mapper)
    : ICommandHandler<RevokeTokenCommand, RevokedTokenResponse>
{
    public async Task<RevokedTokenResponse> Handle(RevokeTokenCommand request, CancellationToken cancellationToken)
    {
        var refreshTkn = await uow.RefreshToken
            .GetAsync(new GetModel<MGH.Core.Infrastructure.Securities.Security.Entities.RefreshToken> { Predicate = r => r.Token == request.Token });
        
        await authBusinessRules.RefreshTokenShouldBeExists(refreshTkn);
        await authBusinessRules.RefreshTokenShouldBeActive(refreshTkn!);
        await uow.RefreshToken.UpdateAsync(refreshTkn,cancellationToken);
        
        return mapper.Map<RevokedTokenResponse>(refreshTkn);
    }
}