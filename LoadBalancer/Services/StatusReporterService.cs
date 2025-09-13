
public class StatusReporterService(HttpClient httpClient, IHealthCheckerService healthCheckerService) : IStatusReporterService
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly IHealthCheckerService _healthCheckerService = healthCheckerService;
    private const string Path = "/status";

    private List<string> _responses = [];

    public async Task<List<string>> GetHealtyBackendStatuses()
    {
        _responses.Clear();
        var urls = _healthCheckerService.GetAllHealthyBackends();

        foreach (var url in urls)
        {
            var response = await _httpClient.GetAsync(url + Path);
            _responses.Add(await response.Content.ReadAsStringAsync());
            
        }
        return _responses;
    }
}