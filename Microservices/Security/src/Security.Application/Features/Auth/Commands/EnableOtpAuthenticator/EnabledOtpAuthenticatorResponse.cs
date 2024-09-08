using MGH.Core.Application.Responses;

namespace Application.Features.Auth.Commands.EnableOtpAuthenticator;

public class EnabledOtpAuthenticatorResponse(string secretKey) : IResponse
{
    public string SecretKey { get; set; } = secretKey;

    public EnabledOtpAuthenticatorResponse() : this(string.Empty)
    {
    }
}
