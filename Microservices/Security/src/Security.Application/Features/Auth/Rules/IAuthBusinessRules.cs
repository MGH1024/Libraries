using MGH.Core.Infrastructure.Securities.Security.Entities;

namespace Application.Features.Auth.Rules;

public interface IAuthBusinessRules
{
    Task UserShouldBeExistsWhenSelected(User user);
    Task RefreshTokenShouldBeExists(RefreshTkn refreshTkn);
    Task RefreshTokenShouldBeActive(RefreshTkn refreshTkn);
    Task UserEmailShouldBeNotExists(string email, CancellationToken cancellationToken);
    Task UserPasswordShouldBeMatch(int id, string password,CancellationToken cancellationToken);
}