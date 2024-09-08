using MGH.Core.Infrastructure.Securities.Security.Entities;

namespace Application.Services.AuthenticatorService;

public interface IAuthenticatorService
{
    public Task<EmailAuthenticator> CreateEmailAuthenticator(User user,CancellationToken cancellationToken);
    public Task<OtpAuthenticator> CreateOtpAuthenticator(User user,CancellationToken cancellationToken);
    public Task<string> ConvertSecretKeyToString(byte[] secretKey);
    public Task SendAuthenticatorCode(User user,CancellationToken cancellationToken);
    public Task VerifyAuthenticatorCode(User user, string authenticatorCode,CancellationToken cancellationToken);
}
