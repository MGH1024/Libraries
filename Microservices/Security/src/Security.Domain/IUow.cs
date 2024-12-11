using Domain.Repositories;
using MGH.Core.Domain;
using MGH.Core.Infrastructure.Persistence;
using MGH.Core.Infrastructure.Persistence.Base;

namespace Domain;

public interface IUow : IUnitOfWork
{
    IOperationClaimRepository OperationClaim { get; }
    IRefreshTokenRepository RefreshToken { get; }
    IUserOperationClaimRepository UserOperationClaim { get; }
    IUserRepository User { get; }
}