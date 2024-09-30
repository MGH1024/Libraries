using Domain.Repositories;
using MGH.Core.Persistence.Base;

namespace Domain;

public interface IUow : IUnitOfWork
{
    IOperationClaimRepository OperationClaim { get; }
    IRefreshTokenRepository RefreshToken { get; }
    IUserOperationClaimRepository UserOperationClaim { get; }
    IUserRepository User { get; }
}