using System.Globalization;
using System.Reflection;
using MGH.Core.CrossCutting.Localizations.RouteConstraints;
using MGH.Core.Infrastructure.ElasticSearch;
using MGH.Core.Infrastructure.ElasticSearch.Models;
using MGH.Core.Infrastructure.Mails;
using MGH.Core.Infrastructure.Mails.MailKitImplementations;
using MGH.Core.Infrastructure.Mails.Models;
using MGH.Core.Infrastructure.Public;
using MGH.Core.Infrastructure.Security.EmailAuthenticator;
using MGH.Core.Infrastructure.Security.JWT;
using MGH.Core.Infrastructure.Security.OtpAuthenticator;
using MGH.Core.Infrastructure.Security.OtpAuthenticator.OtpNet;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using Nest.JsonNetSerializer;
using Newtonsoft.Json;

namespace Infrastructures;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructuresServices(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
        builder.Services.AddTransient<IDateTime, DateTimeService>();
        builder.Services.AddSingleton<IMailService, MailKitMailService>();
        builder.Services.AddScoped<IEmailAuthenticatorHelper, EmailAuthenticatorHelper>();
        builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
        builder.Services.AddScoped<ITokenHelper, JwtHelper>();
        builder.Services.AddScoped<IOtpAuthenticatorHelper, OtpNetOtpAuthenticatorHelper>();
        builder.Services.AddCulture();
        builder.AddElasticSearch();
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
                requestLocalizationOptions.DefaultRequestCulture =
                    new RequestCulture(CultureInfo.GetCultureInfo("en-US"));
                requestLocalizationOptions.SupportedCultures = supportedCultures;
                requestLocalizationOptions.SupportedUICultures = supportedCultures;
                requestLocalizationOptions.RequestCultureProviders.Insert(0, new CultureRequestCultureProvider());
            })
            .AddLocalization(opt => { opt.ResourcesPath = "Resources"; });
    }

    private static async void AddElasticSearch(this WebApplicationBuilder builder)
    {
        const string configurationSection = "ElasticSearchConfig";
        var setting =
            builder.Configuration.GetSection(configurationSection).Get<ElasticSearchConfig>()
            ?? throw new NullReferenceException($"\"{configurationSection}\" " +
                                                $"section cannot found in configuration.");

        var connectionSettings = new ConnectionSettings(new Uri(setting.ConnectionString));
        var client = new ElasticClient(connectionSettings);
        builder.Services.AddSingleton(client);
        builder.Services.AddSingleton<IElasticSearch, ElasticSearchService>(x => new ElasticSearchService(client));

        foreach (var i in setting.Indices)
        {
            if (!(await client.Indices.ExistsAsync(i.IndexName)).Exists)
            {
                await client.Indices.CreateAsync(i.IndexName, selector: se =>
                        se.Settings(a => a.NumberOfReplicas(i.ReplicaCount)
                                .NumberOfShards(i.ShardNumber))
                            .Aliases(x => x.Alias(i.AliasName))
                );
            }
        }
    }
}