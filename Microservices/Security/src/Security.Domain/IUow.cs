using Domain.Repositories;
using MGH.Core.Persistence.Base;

namespace Domain;

public interface IUow : IUnitOfWork
{
    IEmailAuthenticatorRepository EmailAuthenticator { get; }
    IOperationClaimRepository OperationClaim { get; }
    IOtpAuthenticatorRepository OtpAuthenticator { get; }
    IRefreshTokenRepository RefreshToken { get; }
    IUserOperationClaimRepository UserOperationClaim { get; }
    IUserRepository User { get; }
}