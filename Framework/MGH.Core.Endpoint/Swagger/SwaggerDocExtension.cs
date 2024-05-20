using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MGH.Core.Endpoint.Swagger;

public static class SwaggerDocExtension
{
    public static void AddSwaggerDoc(this SwaggerGenOptions swaggerGenOptions,OpenApiInfo openApiInfo)
    {
        swaggerGenOptions.SwaggerDoc(openApiInfo.Version,openApiInfo);
    }
}