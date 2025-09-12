

public class BackgroudHealthCheckerService : BackgroundService
{

    private IHealthCheckerService _healthCheckerService;

    public BackgroudHealthCheckerService(IHealthCheckerService healthCheckerService)
    {
        _healthCheckerService = healthCheckerService;
    }

    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _healthCheckerService.CheckServersActive(stoppingToken);

    }
}