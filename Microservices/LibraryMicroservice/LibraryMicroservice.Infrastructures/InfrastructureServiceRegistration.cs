using System.Reflection;
using Application.Interfaces.Public;
using Application.Models.Email;
using Infrastructures.Public;
using MGH.Core.ElasticSearch;
using MGH.Core.Mailing;
using MGH.Core.Mailing.MailKitImplementations;
using MGH.Core.Security.EmailAuthenticator;
using MGH.Core.Security.JWT;
using MGH.Core.Security.OtpAuthenticator;
using MGH.Core.Security.OtpAuthenticator.OtpNet;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructures;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructuresServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
        services.AddTransient<IEmailSender, EmailSender>();
        services.AddTransient<IDateTime, DateTimeService>();
        services.AddSingleton<IMailService, MailKitMailService>();
        services.AddScoped<IEmailAuthenticatorHelper, EmailAuthenticatorHelper>();
        services.AddSingleton<IElasticSearch, ElasticSearchManager>();
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddScoped<ITokenHelper, JwtHelper>();
        services.AddScoped<IOtpAuthenticatorHelper, OtpNetOtpAuthenticatorHelper>();

        return services;
    }
}