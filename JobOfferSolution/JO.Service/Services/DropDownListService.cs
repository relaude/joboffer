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
    }
}
