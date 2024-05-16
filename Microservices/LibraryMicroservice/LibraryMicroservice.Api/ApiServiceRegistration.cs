using MGH.Swagger;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using MGH.Core.CrossCutting.Exceptions;
using MGH.Core.CrossCutting.Logging;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace Api;

public static class ApiServiceRegistration
{
    public static void AddApiService(this WebApplicationBuilder builder)
    {
        AddLogger(builder);
        builder.Services.AddControllers();
        AddSwagger(builder);
        AddBaseMvc(builder);
        AddCors(builder);
        builder.Services.AddMemoryCache();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddHttpContextAccessor();
    }
    public static void RegisterApp(this WebApplicationBuilder builder)
    {
        var app = builder.Build();
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
        app.UseExceptionMiddleWare();
        app.Run();
    }
    
    private static void AddLogger( WebApplicationBuilder builder)
    {
        builder.CreateLoggerByConfig();    
    }
    private static void AddSwagger( WebApplicationBuilder builder)
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
    private static void AddBaseMvc( WebApplicationBuilder builder)
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
    private static void AddCors( WebApplicationBuilder builder)
    {
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy",
                config => config.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
        });
    }
}