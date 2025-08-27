using System.Reflection;
using System.Text.Json.Serialization;
using Asp.Versioning;
using MGH.Core.CrossCutting.Exceptions;
using MGH.Core.CrossCutting.Localizations.ModelBinders;
using MGH.Core.CrossCutting.Logging;
using MGH.Core.Endpoint.Swagger;
using MGH.Core.Endpoint.Swagger.Models;
using MGH.Core.Infrastructure.HealthCheck;
using MGH.Core.Infrastructure.Securities.Security.Encryption;
using MGH.Core.Infrastructure.Securities.Security.JWT;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.IdentityModel.Tokens;
using Security.Infrastructure;

namespace Security.Endpoint.Api;

public static class ApiServiceRegistration
{
    public static void AddApiService(this IServiceCollection services, IConfiguration configuration, IHostBuilder hostBuilder)
    {
        AddLogger(configuration, hostBuilder);
        services.AddOptions(configuration);
        services.AddSwagger(configuration);
        services.AddCORS(configuration);
        services.AddVersioning();
        services.AddBaseMvc();
        services.AddMemoryCache();
        services.AddHttpContextAccessor();
        services.AddEndpointsApiExplorer();
        services.AddJwt(configuration);
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
    }


    public static void RegisterApp(this WebApplicationBuilder builder)
    {
        var app = builder.Build();
        app.UseRequestLocalization();
        app.UseSwaggerMiddleware();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseHttpsRedirection();
        app.UseCors("CorsPolicy");
        app.MapControllers();
        app.UseExceptionMiddleWare();
        app.UseStaticFiles();
        app.AddHealthCheck();
        app.AddPrometheus();
        app.Run();
    }

    private static void AddOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<TokenOptions>(option => configuration.GetSection(nameof(TokenOptions)).Bind(option));
    }

    private static void AddVersioning(this IServiceCollection services)
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
            .AddMvc() // This is needed for controllers
            .AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'V";
                options.SubstituteApiVersionInUrl = true;
            });
    }

    private static void AddJwt(this IServiceCollection services, IConfiguration configuration)
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

    private static void AddLogger(IConfiguration configuration, IHostBuilder hostBuilder)
    {
        RegisterLogger.CreateLoggerByConfig(configuration, hostBuilder);
    }

    private static void AddBaseMvc(this IServiceCollection services)
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

    private static void AddSwagger(this IServiceCollection services, IConfiguration configuration)
    {
        var swaggerConfig = configuration
            .GetSection(nameof(SwaggerConfig))
            .Get<SwaggerConfig>();

        services.AddSwaggerGen(op =>
        {
            op.AddXmlComments();
            op.AddBearerToken(swaggerConfig.OpenApiSecuritySchemeConfig,
                swaggerConfig.OpenApiReferenceConfig);
            op.AddSwaggerDoc(swaggerConfig.OpenApiInfoConfig);
        });
    }

    private static void AddCORS(this IServiceCollection services, IConfiguration configuration)
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
}