using MGH.Core.Infrastructure.Securities.Security.Entities;

namespace Security.Application.Features.Users.Rules;

public interface IUserBusinessRules
{
    Task UserShouldBeExistsWhenSelected(User user);

    Task UserIdShouldBeExistsWhenSelected(int id, CancellationToken cancellationToken);

    Task UserPasswordShouldBeMatched(User user, string password);

    Task UserEmailShouldNotExistsWhenInsert(string email, CancellationToken cancellationToken);

    Task UserEmailShouldNotExistsWhenUpdate(int id, string email, CancellationToken cancellationToken);
}