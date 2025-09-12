public interface IHealthCheckerService
{

    public Task CheckServersActive(CancellationToken stoppingToken);

    public List<string> GetAllHealthyBackends();

    public string GetNextHealthyBackend();
}