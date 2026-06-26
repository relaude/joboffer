using JO.DataModel.DTOs;

namespace JO.Service.Services.Contracts
{
    public interface IRequestTrackerService
    {
        Task<int> CreateAndEmailMsFormRequest(CandidateMSFormRequestDto dto);
    }
}