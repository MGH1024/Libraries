using Application.Features.Auth.Commands.Register;
using MGH.Core.Infrastructure.Securities.Security.Entities;
using MGH.Core.Infrastructure.Securities.Security.Hashing;

namespace Application.Features.Auth.Extensions;

public static class AuthExtension
{
    public static User ToUser(this RegisterCommand registerCommand, HashingHelperModel hashingHelperModel)
    {
        return new User
        {
            Email = registerCommand.RegisterCommandDto.Email,
            FirstName = registerCommand.RegisterCommandDto.FirstName,
            LastName = registerCommand.RegisterCommandDto.LastName,
            PasswordHash = hashingHelperModel.PasswordHash,
            PasswordSalt = hashingHelperModel.PasswordSalt
        };
    }
}