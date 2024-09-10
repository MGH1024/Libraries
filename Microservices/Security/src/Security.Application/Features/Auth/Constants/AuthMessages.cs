namespace Application.Features.Auth.Constants;

public static class AuthMessages
{
    public const string EmailAuthenticatorDoesNotExists = "Email authenticator does not exists.";
    public const string OtpAuthenticatorDoesNotExists = "Otp authenticator does not exists.";
    public const string AlreadyVerifiedOtpAuthenticatorIsExists = "Already verified otp authenticator is exists.";
    public const string EmailActivationKeyDoesNotExists = "Email Activation Key does not exists.";
    public const string UserDoesNotExists = "User does not exists.";
    public const string UserHaveAlreadyAAuthenticator = "User have already a authenticator.";
    public const string RefreshDoesNotExists = "Refresh does not exists.";
    public const string InvalidRefreshToken = "Invalid refresh token.";
    public const string UserMailAlreadyExists = "User mail already exists.";
    public const string PasswordDoesNotMatch = "Password does not match.";
}
