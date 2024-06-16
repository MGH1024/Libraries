using MGH.Core.Infrastructure.Securities.Security.EmailAuthenticator;
using MGH.Core.Infrastructure.Securities.Security.JWT;
using MGH.Core.Infrastructure.Securities.Security.OtpAuthenticator;
using MGH.Core.Infrastructure.Securities.Security.OtpAuthenticator.OtpNet;
using Microsoft.Extensions.DependencyInjection;

namespace MGH.Core.Infrastructure.Securities.Security;

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
