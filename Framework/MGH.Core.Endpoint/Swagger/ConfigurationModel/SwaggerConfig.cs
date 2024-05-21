namespace MGH.Core.Endpoint.Swagger.ConfigurationModel;

public class SwaggerConfig
{
    public OpenApiSecuritySchemeConfig OpenApiSecuritySchemeConfig { get; set; }
    public OpenApiInfoConfig OpenApiInfoConfig { get; set; }
    public OpenApiReferenceConfig OpenApiReferenceConfig { get; set; }
}