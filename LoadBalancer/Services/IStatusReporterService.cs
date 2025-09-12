public interface IStatusReporterService
{
    public Task<List<string>> GetHealtyBackendStatuses();
}