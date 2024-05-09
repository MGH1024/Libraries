using Serilog;

namespace MGH.Core.CrossCutting.Logging.Serilog;

public abstract class LoggerServiceBase(ILogger logger)
{
    protected ILogger Logger { get; set; } = logger;

    protected LoggerServiceBase() : this(null!)
    {
    }

    public void Verbose(string message) => Logger.Verbose(message);

    public void Fatal(string message) => Logger.Fatal(message);

    public void Info(string message) => Logger.Information(message);

    public void Warn(string message) => Logger.Warning(message);

    public void Debug(string message) => Logger.Debug(message);

    public void Error(string message) => Logger.Error(message);
}
