using MGH.Core.Infrastructure.Security.EmailAuthenticator;
using MGH.Core.Infrastructure.Security.JWT;
using MGH.Core.Infrastructure.Security.OtpAuthenticator;
using MGH.Core.Infrastructure.Security.OtpAuthenticator.OtpNet;
using Microsoft.Extensions.DependencyInjection;

namespace MGH.Core.Infrastructure.Security;

public static class SecurityServiceRegistration
{
    public static IServiceCollection AddSecurityServices(this IServiceCollection services)
    {
        services.AddScoped<ITokenHelper, JwtHelper>();
        services.AddScoped<IEmailAuthenticatorHelper, EmailAuthenticatorHelper>();
        services.AddScoped<IOtpAuthenticatorHelper, OtpNetOtpAuthenticatorHelper>();
        return services;
    }
}
