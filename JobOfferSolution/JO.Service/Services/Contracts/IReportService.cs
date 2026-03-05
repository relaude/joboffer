using JO.DataModel.DTOs;

namespace JO.Service.Services.Contracts
{
    public interface IReportService
    {
        Task<IEnumerable<MainStatusCount>> GetMainStatusCount();
    }
}