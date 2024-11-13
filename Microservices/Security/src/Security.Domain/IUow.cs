using Domain.Repositories;
using MGH.Core.Infrastructure.Persistence.EF.Base;

namespace Domain;

public interface IUow : IUnitOfWork
{
    IOperationClaimRepository OperationClaim { get; }
    IRefreshTokenRepository RefreshToken { get; }
    IUserOperationClaimRepository UserOperationClaim { get; }
    IUserRepository User { get; }
}