using Security.Infrastructure.Contexts;
using MGH.Core.Infrastructure.Persistence.Base;
using Security.Domain;
using Security.Domain.Repositories;

namespace Security.Infrastructure.Repositories;

public class UnitOfWork(
    SecurityDbContext context,
    ITransactionManager<SecurityDbContext> transactionManager,
    IOperationClaimRepository operationClaimRepository,
    IRefreshTokenRepository refreshTokenRepository,
    IUserOperationClaimRepository userOperationClaimRepository,
    IUserRepository userRepository)
    : IUow, IDisposable
{
    public IUserRepository User => userRepository;
    public IRefreshTokenRepository RefreshToken => refreshTokenRepository;
    public IOperationClaimRepository OperationClaim => operationClaimRepository;
    public IUserOperationClaimRepository UserOperationClaim => userOperationClaimRepository;


    public Task<int> CompleteAsync(CancellationToken cancellationToken)
    {
        return context.SaveChangesAsync(cancellationToken);
    }

    public Task BeginTransactionAsync(CancellationToken cancellationToken)
    {
        return transactionManager.BeginTransactionAsync(cancellationToken);
    }

    public Task CommitTransactionAsync(CancellationToken cancellationToken)
    {
        return transactionManager.CommitTransactionAsync(cancellationToken);
    }

    public Task RollbackTransactionAsync(CancellationToken cancellationToken)
    {
        return transactionManager.RollbackTransactionAsync(cancellationToken);
    }

    public void Dispose()
    {
        context.Dispose();
    }
}