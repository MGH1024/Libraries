using AutoMapper;
using MGH.Core.Domain.Buses.Commands;
using MGH.Core.Infrastructure.Securities.Security.Entities;
using Security.Application.Features.Auth.Rules;
using Security.Application.Features.Auth.Services;
using Security.Domain;

namespace Security.Application.Features.Auth.Commands.RegisterUser;

public class RegisterUserCommandHandler(
    IUow uow,
    IAuthService authService,
    IAuthBusinessRules authBusinessRules,
    IMapper mapper)
    : ICommandHandler<RegisterUserCommand, RegisterUserCommandResponse>
{
    public async Task<RegisterUserCommandResponse> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        await authBusinessRules.UserEmailShouldBeNotExists(request.RegisterUserCommandDto.Email, cancellationToken);

        var newUser = mapper.Map<User>(request);
        authService.SetHashPassword(request.RegisterUserCommandDto.Password, newUser);

        var createdUser = await uow.User.AddAsync(newUser, cancellationToken);
        var createdRefreshToken = await authService.CreateRefreshToken(createdUser);
        newUser.RefreshTokens.Add(createdRefreshToken);

        await uow.CompleteAsync(cancellationToken);

        var createdAccessToken = await authService.CreateAccessTokenAsync(createdUser, cancellationToken);
        return new RegisterUserCommandResponse(createdAccessToken.Token, createdAccessToken.Expiration,
            createdRefreshToken.Token, createdRefreshToken.Expires);
    }
}