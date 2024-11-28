using MGH.Core.Infrastructure.Securities.Security.Entities;

namespace MGH.Core.Infrastructure.Securities.Security.JWT;

public interface ITokenHelper
{
    AccessToken CreateToken(User user, IEnumerable<OperationClaim> operationClaims);

    RefreshTkn CreateRefreshToken(User user);
}
