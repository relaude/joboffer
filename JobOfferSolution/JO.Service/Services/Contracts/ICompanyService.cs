using JO.DataModel.View;

namespace JO.Service.Services.Contracts
{
    public interface ICompanyService
    {
        Task UpdateCompanyNames(int companyId, string companyName, IReadOnlyDictionary<int, string> divisionNames);
        Task<int> CreateCompany(JO.DataModel.Entity.Companies company, IReadOnlyCollection<JO.DataModel.Entity.Divisions> divisions);
        Task<List<VwCompanies>> GetVwCompanies();
        Task<JO.DataModel.DTOs.PagedResult<VwCompanies>> GetPagedCompanies(string companyName, string companyCode, int page, int pageSize);
        Task<VwCompanies> GetVwCompany(int companyId);
        Task<List<VwDivisions>> GetVwDivisions(int companyId);
    }
}
