using JO.DataModel.DTOs;

namespace JO.Service.Services.Contracts
{
    public interface IDropDownListService
    {
        Task<IEnumerable<DropdownDto>> GetCandidateStatus();
        Task<List<DropdownDto>> GetCompanies();
        Task<List<DropdownDto>> GetCurrencies();
        Task<IEnumerable<DropdownDto>> GetDeclineReasons();
        Task<IEnumerable<DropdownDto>> GetDepartments();
        Task<List<DropdownDto>> GetDivisionsByCompnayId(int id);
        Task<List<DropdownDto>> GetJobFamilies();
        Task<List<DropdownDto>> GetJobLevels();
        Task<List<DropdownDto>> GetJobPositionGrades();
        Task<IEnumerable<DropdownDto>> GetJobPositions();
        Task<IEnumerable<DropdownDto>> GetMainStatus();
        Task<IEnumerable<DropdownDto>> GetReturnReasons();
        Task<List<DropdownDto>> GetSalaryMatrixByDivisionId(int id);
    }
}