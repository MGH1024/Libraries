using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using MGH.Core.CrossCutting.Exceptions;
using MGH.Core.CrossCutting.Localizations.ModelBinders;
using MGH.Core.CrossCutting.Logging;
using MGH.Core.Endpoint.Swagger;
using MGH.Core.Endpoint.Swagger.Models;
using Microsoft.AspNetCore.Mvc.Formatters;
using Persistence.BackgroundJobs;
using Quartz;

namespace Api;

public static class ApiServiceRegistration
{
    public static void AddApiService(this WebApplicationBuilder builder)
    {
        AddLogger(builder);
        AddBaseMvc(builder);
        AddSwagger(builder);
        AddCors(builder);
        AddQuartzJob(builder);
        builder.Services.AddMemoryCache();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddHttpContextAccessor();
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

    private static void AddLogger(WebApplicationBuilder builder)
    {
        builder.CreateLoggerByConfig();
    }

    private static void AddQuartzJob(WebApplicationBuilder builder)
    {
        builder.Services.AddQuartz(config =>
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
        //builder.Services.AddQuartzHostedService();
    }

    private static void AddSwagger(WebApplicationBuilder builder)
    {
        var swaggerConfig = builder.Configuration
            .GetSection(nameof(SwaggerConfig))
            .Get<SwaggerConfig>();

        builder.Services.AddSwaggerGen(op =>
        {
            op.AddXmlComments();
            op.AddBearerToken(swaggerConfig.OpenApiSecuritySchemeConfig,
                swaggerConfig.OpenApiReferenceConfig);
            op.AddSwaggerDoc(swaggerConfig.OpenApiInfoConfig);
        });
    }

    private static void AddBaseMvc(WebApplicationBuilder builder)
    {
        builder.Services.AddControllers(options =>
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

        builder.Services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });

        builder.Services.AddMvc(setup =>
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

    private static void AddCors(WebApplicationBuilder builder)
    {
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", config => config
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
        });
    }
}