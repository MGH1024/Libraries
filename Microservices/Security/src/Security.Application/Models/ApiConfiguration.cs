namespace Application.Models;

public class ApiConfiguration(string apiDomain, string[] allowedOrigins)
{
    public string ApiDomain { get; set; } = apiDomain;
    public string[] AllowedOrigins { get; set; } = allowedOrigins;

    public ApiConfiguration() : this(string.Empty, Array.Empty<string>())
    {
    }
}
