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
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(30)); // Change the interval as needed

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            var count = Interlocked.Increment(ref executionCount);

            // Create a scope to resolve the scoped service
            using (var scope = _scopeFactory.CreateScope())
            {
                var exchangeService = scope.ServiceProvider.GetRequiredService<IEXChangeService>();
                var res = exchangeService.FetchRateContent(); // Ensure FetchUpdate method is available in IExchangeService
                var output = JsonSerializer.Serialize(res, new JsonSerializerOptions { WriteIndented = true });
                // Console.WriteLine($"Number of items received: {output.Count()}");
            }
        
            _logger.LogInformation("Timed Hosted Service is working. Count: {Count}", count);
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
