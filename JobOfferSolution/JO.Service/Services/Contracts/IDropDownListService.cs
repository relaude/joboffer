using JO.DataModel.DTOs;

namespace JO.Service.Services.Contracts
{
    public interface IDropDownListService
    {
        Task<IEnumerable<DropdownDto>> GetCandidateStatus();
        Task<IEnumerable<DropdownDto>> GetDeclineReasons();
        Task<IEnumerable<DropdownDto>> GetDepartments();
        Task<IEnumerable<DropdownDto>> GetJobPositions();
        Task<IEnumerable<DropdownDto>> GetMainStatus();
        Task<IEnumerable<DropdownDto>> GetReturnReasons();
    }
}