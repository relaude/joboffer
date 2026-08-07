using JO.DataModel.DTOs;
using JO.DataModel.Entity;
using JO.DataModel.View;
using JO.Persistence.DataAccess;
using JO.Service.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace JO.Service.Services
{
    public class BenefitsService : IBenefitsService
    {
        private readonly IDbContextFactory<JobOfferDbContext> _dbContext;

        public BenefitsService(IDbContextFactory<JobOfferDbContext> dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> CreateCompBenPlans(CompBenPlans compBenPlan, List<CompBenItemsDto> compBenItems)
        {
            await using var context = await _dbContext.CreateDbContextAsync();

            compBenPlan.CreatedAt = DateTime.Now;
            await context.CompBenPlans.AddAsync(compBenPlan);
            await context.SaveChangesAsync();

            List<CompBenItems> newItems = new();
            foreach (var item in compBenItems)
            {
                newItems.Add(new CompBenItems
                {
                    Amount = item.Amount,
                    CatId = item.CatId,
                    ItmDesc = item.ItmDesc,
                    ItmName = item.ItmName,
                    Multiplier = item.Multiplier,
                    PlanId = compBenPlan.Id
                });
            }

            await context.CompBenItems.AddRangeAsync(newItems);
            await context.SaveChangesAsync();

            return compBenPlan.Id;
        }

        public async Task<List<CompBenItmCat>> GetCompBenItmCat()
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            return await context.CompBenItmCat.AsNoTracking().ToListAsync();
        }

        public async Task<List<CompBenMRI>> GetCompBenMRI()
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            return await context.CompBenMRI.AsNoTracking().ToListAsync();
        }

        public async Task<List<CompBenFreq>> GetCompBenFreq()
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            return await context.CompBenFreq.AsNoTracking().ToListAsync();
        }

        public async Task<List<CompBenClass>> GetCompBenClass()
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            return await context.CompBenClass.AsNoTracking().ToListAsync();
        }

        public async Task<List<CompBenShift>> GetCompBenShift()
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            return await context.CompBenShift.AsNoTracking().ToListAsync();
        }

        public async Task<List<CompBenSched>> GetCompBenSched()
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            return await context.CompBenSched.AsNoTracking().ToListAsync();
        }

        public async Task<List<CompBenArea>> GetCompBenArea()
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            return await context.CompBenArea.AsNoTracking().ToListAsync();
        }

        public async Task<List<Companies>> GetCompanies()
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            return await context.Companies.AsNoTracking().ToListAsync();
        }

        public async Task<List<CompBenEmpType>> GetCompBenEmpType()
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            return await context.CompBenEmpType.AsNoTracking().ToListAsync();
        }

        public async Task<List<VwCompensationBenefits>> GetCompensationBenefits()
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            return await context.VwCompensationBenefits.AsNoTracking()
                .OrderBy(jo => jo.PackageName).ThenBy(jo => jo.DisplayOrder).ToListAsync();
        }

        public async Task<List<VwCompensationBenefits>> GetCompensationBenefit(int packageId)
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            return await context.VwCompensationBenefits.AsNoTracking()
                .Where(jo => jo.PackageId == packageId)
                .OrderBy(jo => jo.DisplayOrder).ThenBy(jo => jo.ItemName).ToListAsync();
        }

        #region Maintenance Page

        public async Task<List<CompBenPackages>> GetPackages()
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            return await context.CompBenPackages.AsNoTracking().OrderBy(jo => jo.PackageName).ToListAsync();
        }

        public async Task<List<CompBenItems>> GetItems()
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            return await context.CompBenItems.AsNoTracking().OrderBy(jo => jo.ItmName).ToListAsync();
        }

        public async Task<List<CompBenTypes>> GetTypes()
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            return await context.CompBenTypes.AsNoTracking().OrderBy(jo => jo.TypeName).ToListAsync();
        }

        public async Task<List<Frequencies>> GetFrequencies()
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            return await context.Frequencies.AsNoTracking().OrderBy(jo => jo.FrequencyName).ToListAsync();
        }

        public async Task<List<Currencies>> GetCurrencies()
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            return await context.Currencies.AsNoTracking().OrderBy(jo => jo.Currency).ToListAsync();
        }

        public async Task<CompensationBenefits?> GetPackageItem(int id)
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            return await context.CompensationBenefits.AsNoTracking().FirstOrDefaultAsync(jo => jo.Id == id);
        }

        public async Task<int> SavePackage(CompBenPackages package)
        {
            ArgumentNullException.ThrowIfNull(package);
            var name = package.PackageName?.Trim();
            ValidatePackageName(name, package);

            await using var context = await _dbContext.CreateDbContextAsync();
            if (await context.CompBenPackages.AnyAsync(jo => jo.PackageName != null &&
                jo.PackageName.ToLower() == name!.ToLower()))
                throw new InvalidOperationException("A package with the same name already exists.");

            package.PackageName = name;
            package.IsActive ??= true;
            await context.CompBenPackages.AddAsync(package);
            return await context.SaveChangesAsync();
        }

        public async Task<int> UpdatePackage(CompBenPackages package)
        {
            ArgumentNullException.ThrowIfNull(package);
            if (package.Id <= 0)
                throw new ArgumentException("A valid package is required.", nameof(package));
            var name = package.PackageName?.Trim();
            ValidatePackageName(name, package);

            await using var context = await _dbContext.CreateDbContextAsync();
            if (await context.CompBenPackages.AnyAsync(jo => jo.Id != package.Id &&
                jo.PackageName != null && jo.PackageName.ToLower() == name!.ToLower()))
                throw new InvalidOperationException("A package with the same name already exists.");

            var entity = await context.CompBenPackages.FindAsync(package.Id);
            if (entity == null)
                return 0;
            entity.PackageName = name;
            entity.IsActive = package.IsActive;
            return await context.SaveChangesAsync();
        }

        public async Task<int> SavePackageItem(CompensationBenefits packageItem)
        {
            ArgumentNullException.ThrowIfNull(packageItem);
            await using var context = await _dbContext.CreateDbContextAsync();
            await ValidatePackageItem(context, packageItem);
            if (await context.CompensationBenefits.AnyAsync(jo =>
                jo.PackageId == packageItem.PackageId && jo.CompBenItemId == packageItem.CompBenItemId))
                throw new InvalidOperationException("This item is already assigned to the selected package.");

            packageItem.IsActive ??= true;
            packageItem.CreatedAt ??= DateTime.Now;
            await context.CompensationBenefits.AddAsync(packageItem);
            return await context.SaveChangesAsync();
        }

        public async Task<int> UpdatePackageItem(CompensationBenefits packageItem)
        {
            ArgumentNullException.ThrowIfNull(packageItem);
            if (packageItem.Id <= 0)
                throw new ArgumentException("A valid package item is required.", nameof(packageItem));

            await using var context = await _dbContext.CreateDbContextAsync();
            await ValidatePackageItem(context, packageItem);
            if (await context.CompensationBenefits.AnyAsync(jo => jo.Id != packageItem.Id &&
                jo.PackageId == packageItem.PackageId && jo.CompBenItemId == packageItem.CompBenItemId))
                throw new InvalidOperationException("This item is already assigned to the selected package.");

            var entity = await context.CompensationBenefits.FindAsync(packageItem.Id);
            if (entity == null)
                return 0;
            entity.PackageId = packageItem.PackageId;
            entity.CompBenItemId = packageItem.CompBenItemId;
            entity.Amount = packageItem.Amount;
            entity.CurrencyId = packageItem.CurrencyId;
            entity.IsTaxable = packageItem.IsTaxable;
            entity.Tax = packageItem.IsTaxable == true ? packageItem.Tax : 0;
            entity.IsRecurring = packageItem.IsRecurring;
            entity.FrequencyId = packageItem.IsRecurring == true ? packageItem.FrequencyId : null;
            entity.DisplayOrder = packageItem.DisplayOrder;
            entity.IsActive = packageItem.IsActive;
            entity.ModifiedBy = packageItem.ModifiedBy;
            entity.ModifiedAt = DateTime.Now;
            return await context.SaveChangesAsync();
        }

        private static void ValidatePackageName(string? name, CompBenPackages package)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Package name is required.", nameof(package));
            if (name.Length > 150)
                throw new ArgumentException("Package name cannot exceed 150 characters.", nameof(package));
        }

        private static async Task ValidatePackageItem(JobOfferDbContext context, CompensationBenefits packageItem)
        {
            if (packageItem.PackageId.GetValueOrDefault() <= 0 ||
                !await context.CompBenPackages.AnyAsync(jo => jo.Id == packageItem.PackageId))
                throw new ArgumentException("A valid package is required.", nameof(packageItem));
            if (packageItem.CompBenItemId.GetValueOrDefault() <= 0 ||
                !await context.CompBenItems.AnyAsync(jo => jo.Id == packageItem.CompBenItemId))
                throw new ArgumentException("A valid compensation or benefit item is required.", nameof(packageItem));
            if (packageItem.Amount.GetValueOrDefault() < 0 || packageItem.Tax.GetValueOrDefault() < 0)
                throw new ArgumentException("Amount and tax cannot be negative.", nameof(packageItem));
            if (packageItem.DisplayOrder.GetValueOrDefault() < 0)
                throw new ArgumentException("Display order cannot be negative.", nameof(packageItem));
            if (packageItem.CurrencyId.HasValue &&
                !await context.Currencies.AnyAsync(jo => jo.Id == packageItem.CurrencyId))
                throw new ArgumentException("The selected currency is invalid.", nameof(packageItem));
            if (packageItem.IsRecurring == true && packageItem.FrequencyId.GetValueOrDefault() <= 0)
                throw new ArgumentException("Frequency is required for recurring items.", nameof(packageItem));
            if (packageItem.FrequencyId.HasValue &&
                !await context.Frequencies.AnyAsync(jo => jo.Id == packageItem.FrequencyId))
                throw new ArgumentException("The selected frequency is invalid.", nameof(packageItem));
        }

        #endregion
    }
}
