using JO.DataModel.DTOs;

namespace JO.Service.Services.Contracts
{
    public interface IDropDownListService
    {
        Task<List<DropdownDto>> GetCompanies();
        Task<List<DropdownDto>> GetCompBenPackages();
        Task<List<DropdownDto>> GetCurrencies();
        Task<IEnumerable<DropdownDto>> GetDepartments();
        Task<List<DropdownDto>> GetDivisionsByCompnayId(int id);
        Task<List<DropdownDto>> GetJobFamilies();
        Task<List<DropdownDto>> GetJobLevels();
        Task<List<DropdownDto>> GetJobPositionGrades();
        Task<List<DropdownDto>> GetJobPositions();
        Task<List<DropdownDto>> GetJobPositions(int familyId);
        Task<List<DropdownDto>> GetSalaryMatrixByDivisionId(int id);
    }
}