public class ConnectionService(HttpClient httpClient) : IConnectionService
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<bool> ConnectToServer(string url)
    {
         var response = await _httpClient.GetAsync(url + "/connect");
        return response.IsSuccessStatusCode;
    }
}
