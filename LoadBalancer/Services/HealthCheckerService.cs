

public class HealthCheckerService : IHealthCheckerService
{

    private HttpClient _httpClient = new();

    private BackendConfig _backendConfig = new();

    private readonly ILogger<HealthCheckerService> _logger;

    private List<string> _healthyServers = [];

    private int _healthyServersIndex = -1;

    private const string Path = "/status";

    public HealthCheckerService(HttpClient httpClient, BackendConfig backendConfig, ILogger<HealthCheckerService> logger)
    {
        _httpClient = httpClient;
        _backendConfig = backendConfig;
        _logger = logger;
    }

    public async Task CheckServersActive(CancellationToken stoppingToken)
    {
        // logic here to call the backend and remove a server that fails the check

        while (!stoppingToken.IsCancellationRequested)
        {
            foreach (var backendUrl in _backendConfig.BackendUrls)
            {

                // if (api contains OK status)
                try
                {
                    var request = new HttpRequestMessage(HttpMethod.Get, backendUrl + Path);
                    request.Headers.Add("X-LB-SECRET", "my-secret");

                    var response = await _httpClient.SendAsync(request);

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