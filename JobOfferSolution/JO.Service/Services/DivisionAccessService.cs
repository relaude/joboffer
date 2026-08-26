using JO.DataModel.Entity;
using JO.DataModel.View;
using JO.Persistence.DataAccess;
using JO.Service.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace JO.Service.Services
{
    public class DivisionAccessService : IDivisionAccessService
    {
        private readonly IDbContextFactory<JobOfferDbContext> _dbContext;

        public DivisionAccessService(IDbContextFactory<JobOfferDbContext> dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> UpdateUserDivisionAccess(List<int> selectedDivisionIds, int joUserId)
        {
            List<UserDivisionAccess> newAccess = new();

            await using var context = await _dbContext.CreateDbContextAsync();

            var removeAccess = await context.UserDivisionAccess
                .Where(jo => jo.JobOfferUserId == joUserId)
                .ToListAsync();

            foreach (var divisionId in selectedDivisionIds.Distinct())
            {
                newAccess.Add(new UserDivisionAccess { DivisionId = divisionId, JobOfferUserId = joUserId });
            }

            context.UserDivisionAccess.RemoveRange(removeAccess);
            await context.UserDivisionAccess.AddRangeAsync(newAccess);

            return await context.SaveChangesAsync();
        }

        public async Task<List<int>> InitSelectedDivisionIds(List<int> selectedDivisionIds, int joUserId)
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            selectedDivisionIds = await context.UserDivisionAccess
                .Where(jo => jo.JobOfferUserId == joUserId && jo.DivisionId.HasValue)
                .Select(jo => jo.DivisionId!.Value)
                .ToListAsync();

            return selectedDivisionIds;
        }

        public async Task<List<Companies>> GetCompanies()
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            return await context.Companies
                .AsNoTracking()
                .OrderBy(jo => jo.CompanyName)
                .ToListAsync();
        }

        public async Task<List<Divisions>> GetDivisions(int companyId)
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            return await context.Divisions
                .AsNoTracking()
                .Where(jo => jo.CompanyId == companyId)
                .OrderBy(jo => jo.DivisionName)
                .ToListAsync();
        }

        public async Task<VwJobOfferUsers> GetVwJobOfferUser(int userId)
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            return await context.VwJobOfferUsers
                .AsNoTracking()
                .FirstOrDefaultAsync(jo => jo.Id == userId) ?? new VwJobOfferUsers();
        }
    }
}
