using JO.DataModel.DTOs;
using JO.DataModel.Entity;
using JO.DataModel.View;
using JO.Persistence.DataAccess;
using JO.Service.Constants;
using JO.Service.Extensions;
using JO.Service.Services.Contracts;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace JO.Service.Services
{
    public class CandidateService : ICandidateService
    {
        private readonly IDbContextFactory<JobOfferDbContext> _contextFactory;
        public CandidateService(
            IDbContextFactory<JobOfferDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<PagedResult<VwCandidates>> SearchCandidatesAsync(
                int statusId,
                string? candidate,
                int page,
                int pageSize)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();

            var query = context.VwCandidates
                .AsNoTracking();

            if (statusId != 0)
                query = query.Where(jo => jo.CandidateStatus_Id == statusId);

            if (!string.IsNullOrWhiteSpace(candidate))
                query = query.Where(jo =>
                    EF.Functions.Like(jo.LastName, $"%{candidate}%") ||
                    EF.Functions.Like(jo.FirstName, $"%{candidate}%"));

            return await query.ToPagedResultAsync(page, pageSize);
        }

        public async Task<PagedResult<VwCandidates>> CandidatesForJOCretionAsync(
                string? candidate,
                int page,
                int pageSize)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            
            int[] validStatus = [
                JOCandidateStatus.Creation,
                JOCandidateStatus.InProgress,
                JOCandidateStatus.Declined];

            var query = context.VwCandidates
                .AsNoTracking()
                .Where(can => can.CandidateStatus_Id != null &&
                    validStatus.Contains(can.CandidateStatus_Id.Value));

            if (!string.IsNullOrWhiteSpace(candidate))
                query = query.Where(jo =>
                    EF.Functions.Like(jo.LastName, $"%{candidate}%") ||
                    EF.Functions.Like(jo.FirstName, $"%{candidate}%"));

            return await query.ToPagedResultAsync(page, pageSize);
        }

        public async Task<string> GetCandidateEmail(int id)
        {
            var candidate = await GetCandidate(id);
            return candidate.Email;
        }

        public async Task<int> UpdatePersonalInfo(int id,
            string firstName,
            string lastName,
            string email,
            string contactNumber,
            bool isHrod)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            var candidate = await context.Candidates.FindAsync(id);
            candidate.FirstName = firstName;
            candidate.LastName = lastName;
            candidate.Email = email;
            candidate.ContactNumber = contactNumber;
            candidate.IsHROD = isHrod;

            context.Candidates.Update(candidate);
            return await context.SaveChangesAsync();
        }

        public async Task<VwCandidates> GetCandidate(int id)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();

            return await context.VwCandidates
                .AsNoTracking()
                .FirstOrDefaultAsync(x=> x.Id == id);
        }

        public async Task<IEnumerable<VwCandidates>> GetAllCandidates()
        {
            await using var context = await _contextFactory.CreateDbContextAsync();

            return await context.VwCandidates.AsNoTracking().ToListAsync();
        }
    }
}
