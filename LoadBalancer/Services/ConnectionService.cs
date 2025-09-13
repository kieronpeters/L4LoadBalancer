public class ConnectionService(HttpClient httpClient) : IConnectionService
{
    private readonly HttpClient _httpClient = httpClient;
    private const string Path = "/connect";

    public async Task<bool> ConnectToServer(string url)
    {
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url + Path);
            request.Headers.Add("X-LB-SECRET", "my-secret");

            var response = await _httpClient.SendAsync(request);
            return response.IsSuccessStatusCode;
        }
        catch (Exception)
        {
            return false;
        }
        
    }
}
