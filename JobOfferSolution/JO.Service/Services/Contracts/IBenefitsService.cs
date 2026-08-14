using JO.DataModel.DTOs;
using JO.DataModel.Entity;
using JO.DataModel.View;

namespace JO.Service.Services.Contracts
{
    public interface IBenefitsService
    {
        Task<List<VwCompensationBenefits>> GetCompensationBenefit(int packageId);
        Task<List<VwCompensationBenefits>> GetCompensationBenefits();
        Task<List<CompBenPackages>> GetPackages();
        Task<List<CompBenItems>> GetItems();
        Task<List<CompBenTypes>> GetTypes();
        Task<List<Frequencies>> GetFrequencies();
        Task<List<Currencies>> GetCurrencies();
        Task<CompensationBenefits?> GetPackageItem(int id);
        Task<int> SavePackage(CompBenPackages package);
        Task<int> UpdatePackage(CompBenPackages package);
        Task<int> SavePackageItem(CompensationBenefits packageItem);
        Task<int> UpdatePackageItem(CompensationBenefits packageItem);
        Task<List<CompBenEmpType>> GetCompBenEmpType();
        Task<List<Companies>> GetCompanies();
        Task<List<CompBenArea>> GetCompBenArea();
        Task<List<CompBenSched>> GetCompBenSched();
        Task<List<CompBenShift>> GetCompBenShift();
        Task<List<CompBenClass>> GetCompBenClass();
        Task<List<CompBenFreq>> GetCompBenFreq();
        Task<List<CompBenMRI>> GetCompBenMRI();
        Task<List<CompBenItmCat>> GetCompBenItmCat();
        Task<int> CreateCompBenPlans(CompBenPlans compBenPlan, List<CompBenItemsDto> compBenItems);
        Task<List<VwCompBenPlans>> GetVwCompBenPlans();
        Task<List<SalaryGrades>> GetSalaryGrades();
        Task<List<CompBenItems>> GetCompBenItems();
        Task<List<CBPlnHasItem>> GetCBPlnHasItem();
        Task<bool> HasSalaryGrade(int companyId, int gradeId);
        Task<int> SavePackage(CompBenPckgs compBenPckgs, List<CompBenItemsDto> compBenItemsDto);
        Task<List<VwSalaryBands>> GetVwSalaryBands(int companyId);
        Task<List<PckgTemp>> GetPckgTemp();
        Task<List<VwPckgTempHasItms>> GetVwPckgTempHasItms(int tempId);
        Task<int> UpdatePckgTempHasItms(List<VwPckgTempHasItms> tempItems, int modifiedBy);
        Task<List<PckgItemsDto>> GetPckgItemsDto();
        Task<int> AddPckgTemp(PckgTemp newPckgTemp, List<PckgItemsDto> pckgItemsDto);
        Task<List<VwPckgTemp>> GetVwPckgTemp();
    }
}
