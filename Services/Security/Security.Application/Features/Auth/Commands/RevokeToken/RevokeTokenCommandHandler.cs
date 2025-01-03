using AutoMapper;
using MGH.Core.Domain.Buses.Commands;
using MGH.Core.Infrastructure.Persistence.Models.Filters.GetModels;
using Security.Application.Features.Auth.Rules;
using Security.Domain;

namespace Security.Application.Features.Auth.Commands.RevokeToken;

public class RevokeTokenCommandHandler(IAuthBusinessRules authBusinessRules, IUow uow, IMapper mapper) : ICommandHandler<RevokeTokenCommand, RevokedTokenResponse>
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