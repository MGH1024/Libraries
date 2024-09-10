using Domain;
using Domain.Entities.Security;
using MGH.Core.Infrastructure.Public;
using Microsoft.EntityFrameworkCore.Storage;
using Persistence.Contexts;
using Persistence.Repositories.Security;

namespace Persistence.Repositories;

public class UnitOfWork(SecurityDbContext context, IDateTime dateTime) : IUow
{
    private IDbContextTransaction _transaction;
    private EmailAuthenticatorRepository _emailAuthenticatorRepository;
    private OperationClaimRepository _operationClaimRepository;
    private OtpAuthenticatorRepository _otpAuthenticatorRepository;
    private RefreshTokenRepository _refreshTokenRepository;
    private UserOperationClaimRepository _userOperationClaimRepository;
    private UserRepository _userRepository;


    public IEmailAuthenticatorRepository EmailAuthenticator =>
        _emailAuthenticatorRepository ??= new EmailAuthenticatorRepository(context);


    public IOperationClaimRepository OperationClaim =>
        _operationClaimRepository ??= new OperationClaimRepository(context);

    public IOtpAuthenticatorRepository OtpAuthenticator =>
        _otpAuthenticatorRepository ??= new OtpAuthenticatorRepository(context);

    public IRefreshTokenRepository RefreshToken =>
        _refreshTokenRepository ??= new RefreshTokenRepository(context);

    public IUserOperationClaimRepository UserOperationClaim =>
        _userOperationClaimRepository ??= new UserOperationClaimRepository(context);

    public IUserRepository User => _userRepository ??= new UserRepository(context);


    public async Task<int> CompleteAsync(CancellationToken cancellationToken)
    {
        return await context.SaveChangesAsync(cancellationToken);
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken)
    {
        _transaction = await context.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken)
    {
        try
        {
            await context.SaveChangesAsync(cancellationToken);
            await _transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await RollbackTransactionAsync(cancellationToken);
            throw;
        }
        finally
        {
            await _transaction.DisposeAsync();
        }
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _transaction.RollbackAsync(cancellationToken);
        }
        finally
        {
            await _transaction.DisposeAsync();
        }
    }

    public void Dispose()
    {
        context.Dispose();
    }
}