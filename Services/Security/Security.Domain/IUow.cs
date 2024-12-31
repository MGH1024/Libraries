using Domain.Repositories;
using MGH.Core.Infrastructure.Persistence.Base;

namespace Domain;

public interface IUow : IUnitOfWork
{
    IUserRepository User { get; }
    IRefreshTokenRepository RefreshToken { get; }
    IOperationClaimRepository OperationClaim { get; }
    IUserOperationClaimRepository UserOperationClaim { get; }
}