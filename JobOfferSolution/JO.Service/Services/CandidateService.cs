using JO.DataModel.DTOs;
using JO.DataModel.Entity;
using JO.DataModel.View;
using JO.Persistence.DataAccess;
using JO.Service.Services.Contracts;
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
        private readonly IFileUploadService _fileService;
        public CandidateService(
            IDbContextFactory<JobOfferDbContext> contextFactory,
            IFileUploadService fileService)
        {
            _contextFactory = contextFactory;
            _fileService = fileService;
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
