using Application.Features.Auth.Rules;
using Application.Features.OperationClaims.Constants;
using Application.Features.Users.Extensions;
using Application.Services.AuthService;
using AutoMapper;
using Domain;
using MGH.Core.Application.DTOs.Security;
using MGH.Core.Domain.Buses.Commands;
using MGH.Core.Infrastructure.Securities.Security.Entities;
using MGH.Core.Infrastructure.Securities.Security.Hashing;

namespace Application.Features.Auth.Commands.Register;

public class RegisterCommand(UserForRegisterDto userForRegisterDto, string ipAddress) : ICommand<RegisteredResponse>
{
    public UserForRegisterDto UserForRegisterDto { get; set; } = userForRegisterDto;
    public string IpAddress { get; set; } = ipAddress;

    public RegisterCommand() : this(null!, string.Empty)
    {
    }

    public class RegisterCommandHandler(
        IUow uow,
        IAuthService authService,
        AuthBusinessRules authBusinessRules,
        IMapper mapper)
        : ICommandHandler<RegisterCommand, RegisteredResponse>
    {
        public async Task<RegisteredResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            await authBusinessRules.UserEmailShouldBeNotExists(request.UserForRegisterDto.Email);
            var newUser = mapper.Map<User>(request);
            var hashingHelperModel = HashingHelper.CreatePasswordHash(request.UserForRegisterDto.Password);
            newUser.SetHashPassword(hashingHelperModel);

            var createdUser = await uow.User.AddAsync(newUser, cancellationToken);

            var createdRefreshTkn =
                await authService.CreateRefreshToken(createdUser, request.IpAddress, cancellationToken);
            newUser.RefreshTokens.Add(createdRefreshTkn);

            await uow.CompleteAsync(cancellationToken);

            var createdAccessToken = await authService.CreateAccessToken(createdUser, cancellationToken);
            return new RegisteredResponse
            {
                AccessToken = createdAccessToken,
                RefreshTkn = createdRefreshTkn
            };
        }
    }
}