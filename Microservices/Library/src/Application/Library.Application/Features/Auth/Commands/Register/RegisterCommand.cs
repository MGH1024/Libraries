using Application.Features.Auth.Rules;
using Application.Services.AuthService;
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
        AuthBusinessRules authBusinessRules)
        : ICommandHandler<RegisterCommand, RegisteredResponse>
    {

        public async Task<RegisteredResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            await authBusinessRules.UserEmailShouldBeNotExists(request.UserForRegisterDto.Email);

            HashingHelper.CreatePasswordHash(
                request.UserForRegisterDto.Password,
                passwordHash: out byte[] passwordHash,
                passwordSalt: out byte[] passwordSalt
            );
            User newUser =
                new()
                {
                    Email = request.UserForRegisterDto.Email,
                    FirstName = request.UserForRegisterDto.FirstName,
                    LastName = request.UserForRegisterDto.LastName,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt
                };
            var createdUser = await uow.User.AddAsync(newUser, cancellationToken);

            var createdAccessToken = await authService.CreateAccessToken(createdUser, cancellationToken);

            var createdRefreshTkn =
                await authService.CreateRefreshToken(createdUser, request.IpAddress, cancellationToken);
            var addedRefreshTkn =
                await authService.AddRefreshToken(createdRefreshTkn, cancellationToken);

            RegisteredResponse registeredResponse = new()
                { AccessToken = createdAccessToken, RefreshTkn = addedRefreshTkn };
            return registeredResponse;
        }
    }
}