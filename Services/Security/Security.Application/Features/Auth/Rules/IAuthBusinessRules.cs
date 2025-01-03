using MGH.Core.Infrastructure.Securities.Security.Entities;

namespace Security.Application.Features.Auth.Rules;

public interface IAuthBusinessRules
{
    Task UserShouldBeExistsWhenSelected(User user);
    Task RefreshTokenShouldBeExists(RefreshToken refreshToken);
    Task RefreshTokenShouldBeActive(RefreshToken refreshToken);
    Task UserEmailShouldBeNotExists(string email, CancellationToken cancellationToken);
    Task UserPasswordShouldBeMatch(int id, string password,CancellationToken cancellationToken);
}