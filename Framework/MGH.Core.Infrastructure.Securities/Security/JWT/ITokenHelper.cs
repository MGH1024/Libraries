using MGH.Core.Infrastructure.Securities.Security.Entities;

namespace MGH.Core.Infrastructure.Securities.Security.JWT;

public interface ITokenHelper
{
    AccessToken CreateToken(User user, IList<OperationClaim> operationClaims);

    RefreshToken CreateRefreshToken(User user, string ipAddress);
}
