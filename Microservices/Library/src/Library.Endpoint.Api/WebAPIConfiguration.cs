namespace Api;

public class WebApiConfiguration(string apiDomain, string[] allowedOrigins)
{
    public string ApiDomain { get; set; } = apiDomain;
    public string[] AllowedOrigins { get; set; } = allowedOrigins;

    public WebApiConfiguration() : this(string.Empty, Array.Empty<string>())
    {
    }
}
