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
    }
}
