using MGH.Core.Infrastructure.Persistence.Base;
using Security.Domain.Repositories;

namespace Security.Domain;

public interface IUow : IUnitOfWork
{
    IUserRepository User { get; }
    IRefreshTokenRepository RefreshToken { get; }
    IOperationClaimRepository OperationClaim { get; }
    IUserOperationClaimRepository UserOperationClaim { get; }
}