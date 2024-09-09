using MGH.Core.Application.DTOs.Base;

namespace MGH.Core.Application.DTOs.Security;

public class VerifyEmailAuthenticatorDto(string activationKey) : IDto
{
    public string ActivationKey { get; set; } = activationKey;

    public VerifyEmailAuthenticatorDto() : this(string.Empty)
    {
    }
}