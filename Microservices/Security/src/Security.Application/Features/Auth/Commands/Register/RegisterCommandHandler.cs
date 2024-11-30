using Domain;
using AutoMapper;
using MGH.Core.Domain.Buses.Commands;
using Application.Features.Auth.Rules;
using Application.Features.Auth.Services;
using MGH.Core.Infrastructure.Securities.Security.Entities;

namespace Application.Features.Auth.Commands.Register;

public class RegisterCommandHandler(
    IUow uow,
    IAuthService authService,
    IAuthBusinessRules authBusinessRules,
    IMapper mapper)
    : ICommandHandler<RegisterCommand, RegisteredResponse>
{
    public async Task<RegisteredResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        await authBusinessRules.UserEmailShouldBeNotExists(request.RegisterCommandDto.Email, cancellationToken);

        var newUser = mapper.Map<User>(request);
        authService.SetHashPassword(request.RegisterCommandDto.Password, newUser);

        var createdUser = await uow.User.AddAsync(newUser, cancellationToken);
        var createdRefreshToken = await authService.CreateRefreshToken(createdUser);
        newUser.RefreshTokens.Add(createdRefreshToken);

        await uow.CompleteAsync(cancellationToken);

        var createdAccessToken = await authService.CreateAccessTokenAsync(createdUser, cancellationToken);
        return new RegisteredResponse(createdAccessToken.Token, createdAccessToken.Expiration,
            createdRefreshToken.Token, createdRefreshToken.Expires);
    }
}