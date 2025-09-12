

public class HealthCheckerService : IHealthCheckerService
{
    private readonly List<string> _backendUrls =
    [
        "https://localhost:9001",
        "https://localhost:9002",
        "https://localhost:9003"
    ];

    private HttpClient _httpClient = new();

    private readonly ILogger<HealthCheckerService> _logger;

    private List<string> _healthyServers = new();

    private int _healthyServersIndex = -1;

    public HealthCheckerService(HttpClient httpClient, ILogger<HealthCheckerService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;

    }

    public async Task CheckServersActive(CancellationToken stoppingToken)
    {
        // logic here to call the backend and remove a server that fails the check

        while (!stoppingToken.IsCancellationRequested)
        {
            foreach (var backendUrl in _backendUrls)
            {

                // if (api contains OK status)
                try
                {
                    var response = await _httpClient.GetAsync(backendUrl + "/status", stoppingToken);

                    _logger.LogDebug($"response status code from server at {backendUrl} : {response.StatusCode}");

                    if (!response.IsSuccessStatusCode && _healthyServers.Contains(backendUrl))
                    {
                        _healthyServers.Remove(backendUrl);
                    }
                    else if (response.IsSuccessStatusCode && !_healthyServers.Contains(backendUrl))
                    {
                        _healthyServers.Add(backendUrl);
                    }

                }
                catch
                {
                    if (_healthyServers.Contains(backendUrl))
                    {
                        _healthyServers.Remove(backendUrl);
                    }
                }
            }
            _logger.LogDebug($"server count of active routes {_healthyServers.Count}");
            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
        }
        
    }

    public List<string> GetAllHealthyBackends() {
        return _healthyServers;
    }

    public string GetNextHealthyBackend()
    {
        if (_healthyServers.Count == 0)
        {
            return string.Empty;
        }

        // increment index first and test if is valid range index, else reset and retrieve url to use
        // round robin algorithm on server selection
        _healthyServersIndex++;

        if (_healthyServers.Count < _healthyServersIndex + 1)
        {
            _healthyServersIndex = 0;
        }

        return _healthyServers[_healthyServersIndex];
    }
}