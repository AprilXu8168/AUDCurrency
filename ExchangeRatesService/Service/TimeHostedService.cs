using ExchangeRatesService.Models;

namespace ExchangeRatesService.Services
{
    public class TimedHostedService: BackgroundService
    {
        // added debug check
        private readonly ILogger<TimedHostedService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly PeriodicTimer _timer;
        private int _count;

        public TimedHostedService(ILogger<TimedHostedService> logger, IServiceScopeFactory scopeFactory)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _scopeFactory = scopeFactory ?? throw new ArgumentNullException(nameof(scopeFactory));
            _timer = Program.isDebug ? new PeriodicTimer(TimeSpan.FromSeconds(120)) : new PeriodicTimer(TimeSpan.FromHours(120));
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (await _timer.WaitForNextTickAsync(stoppingToken))
            {
                await DoWork(stoppingToken);
            }
            _logger.LogInformation("Timed Hosted Service stopping.");
        }

        private async Task DoWork(CancellationToken stoppingToken)
        {
            const int maxRetries = 5;
            const int delayBetweenRetries = 5000; // 5000ms = 5 seconds

            while (_count < maxRetries)
            {
                using (var scope = _scopeFactory.CreateScope()){
                    var exchangeService = scope.ServiceProvider.GetRequiredService<IExChangeService>();

                    _logger.LogInformation("Timed Hosted Service is working. Count: {Count}", _count++);
                    try
                    {
                        await exchangeService.FetchRateContent();
                        break;
                    }
                    catch (Exception ex)
                    {
                        _count++;
                        _logger.LogError(ex, "An error occurred while doing the work. Attempt {RetryCount} of {MaxRetries}", _count, maxRetries);

                        if (_count >= maxRetries)
                        {
                            _logger.LogError("Max retry attempts reached. Work failed.");
                        }

                        await Task.Delay(delayBetweenRetries); // Wait before retrying
                    }
                }
            }
        }
    }
}