using MGH.Core.Infrastructure.Securities.Security.Entities;
using MGH.Core.Persistence.Base.Repository;

namespace Domain.Repositories;

public interface IOtpAuthenticatorRepository:IRepository<OtpAuthenticator,int>
{
}