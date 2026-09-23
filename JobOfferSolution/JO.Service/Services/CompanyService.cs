using JO.DataModel.View;
using JO.Persistence.DataAccess;
using JO.DataModel.Entity;
using System.ComponentModel.DataAnnotations;
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

        public async Task<int> CreateCompany(Companies company, IReadOnlyCollection<Divisions> divisions)
        {
            ArgumentNullException.ThrowIfNull(company);
            ArgumentNullException.ThrowIfNull(divisions);
            if (string.IsNullOrWhiteSpace(company.CompanyCode) || string.IsNullOrWhiteSpace(company.CompanyName))
                throw new ValidationException("Company code and name are required.");
            if (divisions.Count == 0)
                throw new ValidationException("Add at least one division.");
            if (divisions.Any(division => division is null || string.IsNullOrWhiteSpace(division.DivisionCode) || string.IsNullOrWhiteSpace(division.DivisionName)))
                throw new ValidationException("Every division must have a code and name.");
            if (divisions.Select(division => division.DivisionCode!.Trim()).Distinct(StringComparer.OrdinalIgnoreCase).Count() != divisions.Count)
                throw new ValidationException("Division codes must be unique within the company.");

            // Keep generated IDs out of the form if the transaction fails.
            var newCompany = new Companies { CompanyCode = company.CompanyCode.Trim(), CompanyName = company.CompanyName.Trim() };
            await using var context = await _dbContext.CreateDbContextAsync();
            await using var transaction = await context.Database.BeginTransactionAsync();
            if (await context.Companies.AnyAsync(existing => existing.CompanyCode == newCompany.CompanyCode))
                throw new ValidationException("A company with this code already exists.");
            context.Companies.Add(newCompany);
            await context.SaveChangesAsync();
            context.Divisions.AddRange(divisions.Select(division => new Divisions
            {
                CompanyId = newCompany.Id,
                DivisionCode = division.DivisionCode!.Trim(),
                DivisionName = division.DivisionName!.Trim()
            }));
            await context.SaveChangesAsync();
            await transaction.CommitAsync();
            return newCompany.Id;
        }

        public async Task UpdateCompanyNames(int companyId, string companyName, IReadOnlyDictionary<int, string> divisionNames)
        {
            ArgumentNullException.ThrowIfNull(divisionNames);
            if (string.IsNullOrWhiteSpace(companyName))
                throw new ValidationException("Company name is required.");
            if (divisionNames.Values.Any(string.IsNullOrWhiteSpace))
                throw new ValidationException("Every division must have a name.");

            await using var context = await _dbContext.CreateDbContextAsync();
            var company = await context.Companies.SingleOrDefaultAsync(item => item.Id == companyId)
                ?? throw new ValidationException("Company no longer exists. Return to the company list.");
            var divisions = await context.Divisions.Where(item => item.CompanyId == companyId).ToListAsync();
            if (divisionNames.Keys.Any(id => !divisions.Any(division => division.Id == id)))
                throw new ValidationException("A division no longer belongs to this company. Reload the page and try again.");

            // Update only names on tracked entities; codes and relationships stay untouched.
            company.CompanyName = companyName.Trim();
            foreach (var division in divisions)
                if (divisionNames.TryGetValue(division.Id, out var name))
                    division.DivisionName = name.Trim();

            // A single SaveChanges persists all name changes atomically.
            await context.SaveChangesAsync();
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

        public async Task<JO.DataModel.DTOs.PagedResult<VwCompanies>> GetPagedCompanies(string companyName, string companyCode, int page, int pageSize)
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            var query = context.VwCompanies.AsNoTracking();
            if (!string.IsNullOrWhiteSpace(companyName))
                query = query.Where(company => EF.Functions.Like(company.CompanyName ?? "", $"%{companyName.Trim()}%"));
            if (!string.IsNullOrWhiteSpace(companyCode))
                query = query.Where(company => EF.Functions.Like(company.CompanyCode ?? "", $"%{companyCode.Trim()}%"));

            pageSize = Math.Clamp(pageSize, 1, 100);
            var totalCount = await query.CountAsync();
            page = Math.Clamp(page, 1, Math.Max(1, (int)Math.Ceiling(totalCount / (double)pageSize)));
            return new JO.DataModel.DTOs.PagedResult<VwCompanies>
            {
                Data = await query.OrderBy(company => company.CompanyCode).ThenBy(company => company.Id)
                    .Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(),
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
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
