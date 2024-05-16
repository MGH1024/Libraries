using System.Text.Json.Serialization;
using MGH.Swagger;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.OpenApi.Models;
using Serilog;

namespace Api;

public static class ApiServiceRegistration
{
    public static void CreateLoggerByConfig(this  WebApplicationBuilder builder )
    {
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .WriteTo.Async(wt=>wt.Console())
            //.WriteTo.File(,)
            .CreateLogger();
        builder.Host.UseSerilog((hostingContext, loggerConfiguration) =>
            loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration));
        Log.Information("The global logger has been configured");
    }

    public static void AddSwagger(this WebApplicationBuilder builder)
    {
        builder.Services.AddSwaggerGen(op =>
        {
            op.AddXmlComments();
            op.AddBearerToken(new OpenApiSecurityScheme
            {
                Description = "just copy token in value TextBox",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            });
            op.AddSwaggerDoc(new OpenApiInfo
            {
                Title = "Library microservice",
                Version = "v1",
                Description = "API"
            });
        });
    }

    public static void AddBaseMvc(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers()
            .ConfigureApiBehaviorOptions(opt =>
            {
                // opt.InvalidModelStateResponseFactory = (ctx) =>
                // {
                //     var modelState = ctx.ModelState;
                //     var errors = modelState
                //         .Keys
                //         .SelectMany(key => modelState[key]?
                //             .Errors
                //             .Select(err => new ValidationError(key.Replace("$.", ""), err.ErrorMessage)));
                //     throw new CustomValidationException(errors);
                // };
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

    public static void AddCors(this WebApplicationBuilder builder)
    {
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy",
                config => config.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
        });
    }
    public static void RegisterApp (this WebApplication app )
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseAuthentication();
        app.UseAuthorization();
        app.UseHttpsRedirection();
        app.UseCors("CorsPolicy");
        app.MapControllers();
    }
}