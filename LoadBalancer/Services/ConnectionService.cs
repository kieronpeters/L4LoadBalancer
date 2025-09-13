public class ConnectionService(HttpClient httpClient) : IConnectionService
{
    private readonly HttpClient _httpClient = httpClient;
    private const string Path = "/connect";

    public async Task<bool> ConnectToServer(string url)
    {
        var response = await _httpClient.GetAsync(url + Path);
        return response.IsSuccessStatusCode;
    }
}
