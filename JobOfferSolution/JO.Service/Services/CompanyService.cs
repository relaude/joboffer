using JO.DataModel.View;
using JO.Persistence.DataAccess;
using JO.Service.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace JO.Service.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly IDbContextFactory<JobOfferDbContext> _dbContext;

        public CompanyService(IDbContextFactory<JobOfferDbContext> dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<VwCompanies> GetVwCompany(int companyId)
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            return await context.VwCompanies
                .AsNoTracking()
                .FirstOrDefaultAsync(jo => jo.Id == companyId) ?? new VwCompanies();
        }

        public async Task<List<VwCompanies>> GetVwCompanies()
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            return await context.VwCompanies.AsNoTracking().ToListAsync();
        }

        public async Task<List<VwDivisions>> GetVwDivisions(int companyId)
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            return await context.VwDivisions
                .AsNoTracking()
                .Where(jo => jo.CompanyId == companyId)
                .OrderBy(jo => jo.DivisionName)
                .ToListAsync();
        }
    }
}
