using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NuGet.Protocol;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ExchangeRatesService.Services
{
    public class TimedHostedService : IHostedService, IDisposable
    {
        private int executionCount = 0;
        private readonly ILogger<TimedHostedService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        private Timer _timer;

        public TimedHostedService(ILogger<TimedHostedService> logger, IServiceScopeFactory scopeFactory)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _scopeFactory = scopeFactory ?? throw new ArgumentNullException(nameof(scopeFactory));
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Hosted Service running.");

            // Start the timer to call DoWork method periodically
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(900)); // Change the interval as needed

            return Task.CompletedTask;
        }

        private async void DoWork(object state)
        {
                    int retryCount = 0;
        const int maxRetries = 5;
        const int delayBetweenRetries = 5000; // 5000ms = 5 seconds

        while (retryCount < maxRetries)
        {
            try
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    // Call your service methods here
                    var server = scope.ServiceProvider.GetRequiredService<IEXChangeService>();
                    await server.FetchRateContent();

                    _logger.LogInformation("Work completed successfully.");
                    break; // Exit the loop if successful
                }
            }
            catch (Exception ex)
            {
                retryCount++;
                _logger.LogError(ex, "An error occurred while doing the work. Attempt {RetryCount} of {MaxRetries}", retryCount, maxRetries);

                if (retryCount >= maxRetries)
                {
                    _logger.LogError("Max retry attempts reached. Work failed.");
                    break;
                }

                await Task.Delay(delayBetweenRetries); // Wait before retrying
            }
        }
    }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Hosted Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
