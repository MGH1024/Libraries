using Domain.Entities.Libraries;
using Domain.Security;
using MGH.Core.Persistence.Base;

namespace Domain;

public interface IUow : IUnitOfWork
{
    ILibraryRepository Library { get; }
    IOutBoxRepository OutBox { get; }
    IEmailAuthenticatorRepository EmailAuthenticator { get; }
    IOperationClaimRepository OperationClaimRepository { get; }
    IOtpAuthenticatorRepository OtpAuthenticator { get; }
    IRefreshTokenRepository RefreshToken { get; }
    IUserOperationClaimRepository OperationClaim { get; }
    IUserRepository User { get; }
}