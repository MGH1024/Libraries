using Application.Features.Auth.Commands.Register;
using MGH.Core.Infrastructure.Securities.Security.Entities;
using MGH.Core.Infrastructure.Securities.Security.Hashing;
using Nest;

namespace Application.Features.Auth.Extensions;

public static class AuthExtension
{
    public static User ToUser(this RegisterCommand registerCommand, HashingHelperModel hashingHelperModel)
    {
        return new User
        {
            Email = registerCommand.UserForRegisterDto.Email,
            FirstName = registerCommand.UserForRegisterDto.FirstName,
            LastName = registerCommand.UserForRegisterDto.LastName,
            PasswordHash = hashingHelperModel.PasswordHash,
            PasswordSalt = hashingHelperModel.PasswordSalt
        };
    }
}