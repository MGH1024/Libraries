namespace Library.Endpoint.Worker.Inbox
{
    public class InboxWorker : BackgroundService
    {
        private readonly ILogger<InboxWorker> _logger;

        public InboxWorker(ILogger<InboxWorker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (_logger.IsEnabled(LogLevel.Information))
                {
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                }
                await Task.Delay(10000, stoppingToken);
            }
        }
    }
}
