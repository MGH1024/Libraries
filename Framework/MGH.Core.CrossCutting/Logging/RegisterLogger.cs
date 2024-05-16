using Microsoft.AspNetCore.Builder;
using Serilog;

namespace MGH.Core.CrossCutting.Logging;

public static class RegisterLogger
{
    public static void CreateLoggerByConfig(this  WebApplicationBuilder builder )
    {
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .WriteTo.Async(wt=>wt.Console())
            .CreateLogger();
        builder.Host.UseSerilog((hostingContext, loggerConfiguration) =>
            loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration));
        Log.Information("The global logger has been configured");
    }
}