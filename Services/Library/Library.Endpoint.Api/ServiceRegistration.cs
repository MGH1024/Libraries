using Asp.Versioning;
using Library.Endpoint.Api.Options;
using MGH.Core.CrossCutting.Localizations.ModelBinders;
using MGH.Core.Infrastructure.Securities.Security.Encryption;
using MGH.Core.Infrastructure.Securities.Security.JWT;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System.Text.Json.Serialization;

namespace Library.Endpoint.Api;

/// <summary>
/// ApiServiceRegistration
/// </summary>
public static class ServiceRegistration
{
    internal static void AddApiOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<TokenOptions>(option => configuration.GetSection(nameof(TokenOptions)).Bind(option));
    }

    internal static void AddVersioning(this IServiceCollection services)
    {
        services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 2);
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ApiVersionReader = ApiVersionReader.Combine(
                    new UrlSegmentApiVersionReader(),
                    new HeaderApiVersionReader("X-Api-Version"));
            })
            .AddMvc()
            .AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'V";
                options.SubstituteApiVersionInUrl = true;
            });
    }

    internal static void AddJwt(this IServiceCollection services, IConfiguration configuration)
    {
        var tokenOptions =
            configuration.GetSection("TokenOptions").Get<TokenOptions>()
            ?? throw new InvalidOperationException("TokenOptions section cannot found in configuration.");
        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = tokenOptions.Issuer,
                    ValidAudience = tokenOptions.Audience,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = SecurityKeyHelper.CreateSecurityKey(tokenOptions.SecurityKey)
                };
            });
    }

    internal static void AddBaseMvc(this IServiceCollection services)
    {
        services.AddControllers(options =>
            {
                options.ModelBinderProviders.Insert(0, new DateTimeModelBinderProvider());
                options.ValueProviderFactories.Insert(0, new SeparatedQueryStringValueProviderFactory(","));
                options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
            })
            .AddJsonOptions(opts =>
            {
                var enumConvertor = new JsonStringEnumConverter();
                opts.JsonSerializerOptions.Converters.Add(new DateTimeConverter());
                opts.JsonSerializerOptions.Converters.Add(enumConvertor);
            });

        services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });

        services.AddMvc(setup =>
            {
                setup.ReturnHttpNotAcceptable = true;
                setup.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());
            })
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

        services.AddHttpContextAccessor();
    }

    internal static void AddCORS(this IServiceCollection services, IConfiguration configuration)
    {
        var allowedOrigins = configuration.GetSection("Cors:AllowedOrigins").Get<string[]>();

        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", builder =>
            {
                if (allowedOrigins != null && allowedOrigins.Length > 0)
                {
                    builder.WithOrigins(allowedOrigins)
                           .AllowAnyMethod()
                           .AllowAnyHeader()
                           .AllowCredentials();
                }
                else
                {
                    // Fallback for development
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                }
            });
        });
    }

    internal static IServiceCollection AddSwaggerService(this IServiceCollection services, IConfiguration config)
    {
        var settings = config.GetSection("Swagger").Get<SwaggerSettings>();

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc(settings.Version, new OpenApiInfo
            {
                Title = settings.Title,
                Version = settings.Version,
                Description = settings.Description,
                Contact = new OpenApiContact
                {
                    Name = settings.Contact?.Name,
                    Email = settings.Contact?.Email,
                    Url = settings.Contact?.Url != null ? new Uri(settings.Contact.Url) : null
                },
                License = new OpenApiLicense
                {
                    Name = settings.License?.Name,
                    Url = settings.License?.Url != null ? new Uri(settings.License.Url) : null
                }
            });

            // Bearer token security
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = settings.BearerDescription
            });

            //c.AddSecurityRequirement(new OpenApiSecurityRequirement
            //{
            //    {
            //        new OpenApiSecurityScheme
            //        {
            //            Reference = new OpenApiReference
            //            {
            //                Type = ReferenceType.SecurityScheme,
            //                Id = "Bearer"
            //            }
            //        },
            //        Array.Empty<string>()
            //    }
            //});
        });

        return services;
    }

    internal static void UseSwaggerMiddleWare(this WebApplication app, IConfiguration config)
    {
        var settings = config.GetSection("Swagger").Get<SwaggerSettings>();

        //if (app.Environment.IsDevelopment())
        //{
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"/swagger/{settings.Version}/swagger.json", $"{settings.Title} {settings.Version}");
                c.RoutePrefix = settings.RoutePrefix;
                c.DocumentTitle = $"{settings.Title} Documentation";
            });
        //s}
    }
}
