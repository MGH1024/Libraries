using MGH.Core.CrossCutting.Logging;

namespace Library.Worker.Outbox;

public static class WorkerServiceRegistration
{
    public static async void RegisterApp(this HostApplicationBuilder builder) 
    {
        RegisterLogger.CreateLoggerByConfig(builder.Configuration);
        builder.Services.AddHostedService<Worker>();
        var host = builder.Build();
        await host.RunAsync();
    }
}