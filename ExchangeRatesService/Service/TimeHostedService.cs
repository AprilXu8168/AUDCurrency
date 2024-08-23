namespace ExchangeRatesService.Services;

// using a primary constructor
public class TimedHostedService(ILogger<TimedHostedService> logger, IExChangeService exchangeService)
    : BackgroundService
{
    // added debug check
    private readonly PeriodicTimer _timer = new(Program.isDebug ? TimeSpan.FromSeconds(120) : TimeSpan.FromHours(1));
    private int _count;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Timed Hosted Service starting.");
        
        while (await _timer.WaitForNextTickAsync(stoppingToken))
        {
            await DoWork();
        }

        logger.LogInformation("Timed Hosted Service stopping.");
    }
    
    
    private async Task DoWork()
    {
        logger.LogInformation("Timed Hosted Service is working. Count: {Count}", _count++);

        try
        {
            // any further logic should be inside the called function. keep the hosted service simple
            await exchangeService.FetchRateContent();
        }
        catch (Exception e)
        {
            logger.LogError("Timed Hosted Service failed: {error}", e);
            // do not throw, as we want the service to try again on the next interval
        }

    }
}
