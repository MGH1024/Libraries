using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using MGH.Core.CrossCutting.Exceptions;
using MGH.Core.CrossCutting.Localizations.ModelBinders;
using MGH.Core.CrossCutting.Logging;
using MGH.Core.Endpoint.Swagger.Swagger;
using MGH.Core.Endpoint.Swagger.Swagger.Models;
using Microsoft.AspNetCore.Mvc.Formatters;
using Persistence.BackgroundJobs;
using Quartz;

namespace Api;

public static class ApiServiceRegistration
{
    public static void AddApiService(this IServiceCollection services, IConfiguration configuration,
        IHostBuilder hostBuilder)
    {
        AddLogger(configuration, hostBuilder);
        services.AddSwagger(configuration);
        services.AddCors();
        services.AddBaseMvc();
        services.AddQuartzJob();
        services.AddMemoryCache();
        services.AddHttpContextAccessor();
        services.AddEndpointsApiExplorer();
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
        app.Run();
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
    }

    private static void AddQuartzJob(this IServiceCollection services)
    {
        services.AddQuartz(config =>
        {
            var jobKey = new JobKey(nameof(ProcessOutboxMessagesJob));

            config.AddJob<ProcessOutboxMessagesJob>(jobKey)
                .AddTrigger(
                    trigger =>
                        trigger.ForJob(jobKey)
                            .WithSimpleSchedule(
                                schedule =>
                                    schedule.WithIntervalInSeconds(5)
                                        .RepeatForever()));
            config.UseMicrosoftDependencyInjectionJobFactory();
        });
        //services.AddQuartzHostedService();
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

    private static void AddCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", config => config
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
        });
    }
}