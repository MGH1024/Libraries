using MGH.Core.Persistence.Base.Repository;
using MGH.Core.Infrastructure.Securities.Security.Entities;

namespace Domain.Repositories;

public interface IEmailAuthenticatorRepository : IRepository<EmailAuthenticator, int>
{
}