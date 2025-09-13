
using System.Linq.Expressions;

public class StatusReporterService(HttpClient httpClient, IHealthCheckerService healthCheckerService) : IStatusReporterService
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly IHealthCheckerService _healthCheckerService = healthCheckerService;
    private const string Path = "/status";

    private List<string> _responses = [];

    public async Task<List<string>> GetHealtyBackendStatuses()
    {
        _responses.Clear();
        try
        {
            var urls = _healthCheckerService.GetAllHealthyBackends();
            foreach (var url in urls)
            {
                var request = new HttpRequestMessage(HttpMethod.Get, url + Path);
                request.Headers.Add("X-LB-SECRET", "my-secret");

                var response = await _httpClient.SendAsync(request);
                _responses.Add(await response.Content.ReadAsStringAsync());

            }
            return _responses;
        }
        catch (Exception)
        {
            return _responses;
        }
        
    }
}