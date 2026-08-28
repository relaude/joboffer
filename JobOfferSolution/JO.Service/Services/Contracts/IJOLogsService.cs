using JO.DataModel.View;

namespace JO.Service.Services.Contracts
{
    public interface IJOLogsService
    {
        Task<List<VwJOActionLogs>> GetVwJOActionLogs(int jobOfferId);
    }
}