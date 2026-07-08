using JO.DataModel.DTOs;
using JO.DataModel.Entity;
using JO.Persistence.DataAccess;
using JO.Service.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace JO.Service.Services
{
    public class DropDownListService : IDropDownListService
    {
        private readonly IDbContextFactory<JobOfferDbContext> _contextFactory;
        public DropDownListService(IDbContextFactory<JobOfferDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<List<DropdownDto>> GetCompBenPackages()
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            return await context.CompBenPackages
                .AsNoTracking()
                .Where(jo => jo.IsActive == true)
                .Select(jo => new DropdownDto
                {
                    Id = jo.Id,
                    Value = jo.PackageName
                }).ToListAsync();
        }

        public async Task<List<DropdownDto>> GetSalaryMatrixByDivisionId(int id)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            return await context.VwSalaryMatrix
                .AsNoTracking()
                .Where(jo => jo.DivisionId == id)
                .Select(jo => new DropdownDto
                {
                    Id = jo.Id,
                    Value = jo.MatrixCode
                }).ToListAsync();
        }

        public async Task<List<DropdownDto>> GetDivisionsByCompnayId(int id)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Divisions
                .AsNoTracking()
                .Where(jo => jo.CompanyId == id)
                .OrderBy(jo => jo.DivisionCode)
                .Select(jo => new DropdownDto
                {
                    Id = jo.Id,
                    Value = $"{jo.DivisionCode} - {jo.DivisionName}"
                }).ToListAsync();
        }

        public async Task<List<DropdownDto>> GetJobPositionGrades()
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            return await context.JobPositionGrades
                .AsNoTracking()
                .OrderBy(jo => jo.PositionGrade)
                .Select(jo => new DropdownDto
                {
                    Id = jo.Id,
                    Value = jo.PositionGrade
                }).ToListAsync();
        }

        public async Task<List<DropdownDto>> GetJobFamilies()
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            return await context.JobFamilies
                .AsNoTracking()
                .OrderBy(jo => jo.JobFamilyName)
                .Select(jo => new DropdownDto
                {
                    Id = jo.Id,
                    Value = jo.JobFamilyName
                }).ToListAsync();
        }

        public async Task<List<DropdownDto>> GetJobLevels()
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            return await context.JobLevels
                .AsNoTracking()
                .OrderBy(jo => jo.JobLevelName)
                .Select(jo => new DropdownDto
                {
                    Id = jo.Id,
                    Value = jo.JobLevelName
                }).ToListAsync();
        }

        public async Task<List<DropdownDto>> GetCurrencies()
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Currencies
                .AsNoTracking()
                .OrderBy(jo => jo.Currency)
                .Select(jo => new DropdownDto
                {
                    Id = jo.Id,
                    Value = $"{jo.Currency} - {jo.Description}"
                }).ToListAsync();
        }

        public async Task<List<DropdownDto>> GetCompanies()
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Companies
                .AsNoTracking()
                .OrderBy(jo => jo.CompanyCode)
                .Select(jo => new DropdownDto
                {
                    Id = jo.Id,
                    Value = $"{jo.CompanyCode} - {jo.CompanyName}"
                }).ToListAsync();
        }
        public async Task<IEnumerable<DropdownDto>> GetMainStatus()
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            return await context.MainStatus
                .AsNoTracking()
                .OrderBy(jo => jo.OrderBy)
                .Select(jo => new DropdownDto
                {
                    Id = jo.Id,
                    Value = jo.StatusName
                }).ToListAsync();
        }

        public async Task<IEnumerable<DropdownDto>> GetCandidateStatus()
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            return await context.CandidateStatus
                .AsNoTracking()
                .OrderBy(jo => jo.OrderBy)
                .Select(jo => new DropdownDto
                {
                    Id = jo.Id,
                    Value = jo.StatusName
                }).ToListAsync();
        }
        public async Task<IEnumerable<DropdownDto>> GetJobPositions()
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            return await context.JobPositions
                .AsNoTracking()
                .Select(jo => new DropdownDto
                {
                    Id = jo.Id,
                    Value = jo.PositionName
                }).ToListAsync();
        }

        public async Task<IEnumerable<DropdownDto>> GetDepartments()
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Departments
                .AsNoTracking()
                .Select(jo => new DropdownDto
                {
                    Id = jo.Id,
                    Value = jo.DepartmentName
                }).ToListAsync();
        }

        public async Task<IEnumerable<DropdownDto>> GetDeclineReasons()
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            return await context.DeclineReasons
                .AsNoTracking()
                .Select(jo => new DropdownDto
                {
                    Id = jo.Id,
                    Value = jo.Reason
                }).ToListAsync();
        }

        public async Task<IEnumerable<DropdownDto>> GetReturnReasons()
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            return await context.ReturnReasons
                .AsNoTracking()
                .Select(jo => new DropdownDto
                {
                    Id = jo.Id,
                    Value = jo.Reason
                }).ToListAsync();
        }
    }
}
