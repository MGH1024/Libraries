using System.Globalization;
using System.Reflection;
using Application.Interfaces.Public;
using Application.Models.Email;
using Infrastructures.Public;
using MGH.Core.CrossCutting.Localizations.RouteConstraints;
using MGH.Core.Infrastructure.ElasticSearch;
using MGH.Core.Infrastructure.Mails;
using MGH.Core.Infrastructure.Mails.MailKitImplementations;
using MGH.Core.Infrastructure.Security.EmailAuthenticator;
using MGH.Core.Infrastructure.Security.JWT;
using MGH.Core.Infrastructure.Security.OtpAuthenticator;
using MGH.Core.Infrastructure.Security.OtpAuthenticator.OtpNet;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructures;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructuresServices(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
        builder.Services.AddTransient<IEmailSender, EmailSender>();
        builder.Services.AddTransient<IDateTime, DateTimeService>();
        builder.Services.AddSingleton<IMailService, MailKitMailService>();
        builder.Services.AddScoped<IEmailAuthenticatorHelper, EmailAuthenticatorHelper>();
        builder.Services.AddSingleton<IElasticSearch, ElasticSearchManager>();
        builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
        builder.Services.AddScoped<ITokenHelper, JwtHelper>();
        builder.Services.AddScoped<IOtpAuthenticatorHelper, OtpNetOtpAuthenticatorHelper>();
        builder.Services.AddCulture();

        return builder.Services;
    }
    
    private static void AddCulture(this IServiceCollection services)
    {
        var supportedCultures = CultureInfo.GetCultures(CultureTypes.AllCultures);

        services
            .Configure<RouteOptions>(routeOptions =>
            {
                routeOptions.ConstraintMap.Add(nameof(CultureRouteConstraint), typeof(CultureRouteConstraint));
            })
            .Configure<RequestLocalizationOptions>(requestLocalizationOptions =>
            {
                requestLocalizationOptions.DefaultRequestCulture = new RequestCulture(CultureInfo.GetCultureInfo("en-US"));
                requestLocalizationOptions.SupportedCultures = supportedCultures;
                requestLocalizationOptions.SupportedUICultures = supportedCultures;
                requestLocalizationOptions.RequestCultureProviders.Insert(0, new CultureRequestCultureProvider());
            })
            .AddLocalization(opt => { opt.ResourcesPath = "Resources"; });
    }
}