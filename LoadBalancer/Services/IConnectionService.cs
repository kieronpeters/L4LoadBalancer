public interface IConnectionService
{
    public Task<bool> ConnectToServer(string url);
}