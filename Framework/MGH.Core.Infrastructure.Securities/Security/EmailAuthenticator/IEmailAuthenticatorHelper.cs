namespace MGH.Core.Infrastructure.Securities.Security.EmailAuthenticator;

public interface IEmailAuthenticatorHelper
{
    public Task<string> CreateEmailActivationKey();
    public Task<string> CreateEmailActivationCode();
}
