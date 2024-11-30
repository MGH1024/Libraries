using Domain;
using AutoMapper;
using MGH.Core.Domain.Buses.Commands;
using Application.Features.Auth.Rules;
using Application.Features.Auth.Services;
using Application.Features.Users.Extensions;
using MGH.Core.Infrastructure.Securities.Security.Hashing;
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
        var hashingHelperModel = HashingHelper.CreatePasswordHash(request.RegisterCommandDto.Password);
        newUser.SetHashPassword(hashingHelperModel);

        var createdUser = await uow.User.AddAsync(newUser, cancellationToken);

        var createdRefreshTkn = await authService.CreateRefreshToken(createdUser, cancellationToken);
        newUser.RefreshTokens.Add(createdRefreshTkn);

        await uow.CompleteAsync(cancellationToken);

        var createdAccessToken = await authService.CreateAccessTokenAsync(createdUser, cancellationToken);
        return new RegisteredResponse(createdAccessToken, createdRefreshTkn);
    }
}