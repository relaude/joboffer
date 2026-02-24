using JO.DataModel.DTOs;

namespace JO.Service.Services.Contracts
{
    public interface IDropDownListService
    {
        Task<IEnumerable<DropdownDto>> GetDepartments();
        Task<IEnumerable<DropdownDto>> GetJobPositions();
    }
}