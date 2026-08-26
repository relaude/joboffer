using JO.DataModel.View;

namespace JO.Service.Services.Contracts
{
    public interface ICompanyService
    {
        Task<List<VwCompanies>> GetVwCompanies();
        Task<VwCompanies> GetVwCompany(int companyId);
        Task<List<VwDivisions>> GetVwDivisions(int companyId);
    }
}