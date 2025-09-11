using System.Collections.Concurrent;

public class StatusReporterService
{
    private HttpClient _httpClient;
    private HealthCheckerService _healthCheckerService;

    private List<string> _responses = [];

    public StatusReporterService(HttpClient httpClient, HealthCheckerService healthCheckerService)
    {
        _httpClient = httpClient;
        _healthCheckerService = healthCheckerService;
    }


    public async Task<List<string>> GetHealtyBackendStatuses()
    {
        var urls = _healthCheckerService.GetAllHealthyBackends();

        foreach (var url in urls)
        {
            var response = await _httpClient.GetAsync(url);
            _responses.Add(response.Content.ToString()!);
            
        }
        return _responses;
    }
}