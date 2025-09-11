using System.Collections.Concurrent;

public class HealthCheckerService : BackgroundService
{
    private readonly List<String> _backendUrls =
    [
        "http://localhost:9001",
        "http://localhost:9002",
        "http://localhost:9003"
    ];

    private HttpClient _httpClient = new();

    private List<string> _healthyServers = new();

    private int _healthyServersIndex = -1;


    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // logic here to call the backend and remove a server that fails the check

        foreach (var backendUrl in _backendUrls)
        {
            // if (api contains OK status)

            var response = await _httpClient.GetAsync(backendUrl + "/status", stoppingToken);

            if (!response.IsSuccessStatusCode && _healthyServers.Contains(backendUrl))
            {
                _healthyServers.Remove(backendUrl);
            }
            else if (response.IsSuccessStatusCode && !_healthyServers.Contains(backendUrl))
            {
                _healthyServers.Add(backendUrl);
            }
        }
    }

    public List<string> GetAllHealthyBackends() {
        return _healthyServers;
    };

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