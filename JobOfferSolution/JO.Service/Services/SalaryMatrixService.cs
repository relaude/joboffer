using JO.DataModel.Entity;
using JO.DataModel.View;
using JO.Persistence.DataAccess;
using JO.Service.Constants;
using JO.Service.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace JO.Service.Services
{
    public class SalaryMatrixService : ISalaryMatrixService
    {
        private readonly IDbContextFactory<JobOfferDbContext> _contextFactory;
        public SalaryMatrixService(IDbContextFactory<JobOfferDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<List<VwSalaryMatrixBand>> GetSalaryBandsByJOId(int jobOfferId)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();

            var legal = await context.LegalEntities.FirstOrDefaultAsync(jo=> jo.JobOfferId==jobOfferId);

            return await context.VwSalaryMatrixBand
                .Where(jo => jo.SalaryMatrixId == legal.MatrixId)
                .ToListAsync();
        }

        public async Task<List<VwSalaryMatrixBand>> GetSalaryBands(int matrixId)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            return await context.VwSalaryMatrixBand
                .Where(jo => jo.SalaryMatrixId == matrixId)
                .ToListAsync();
        }

        public async Task<VwSalaryMatrix> GetMatrix(int matrixId)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            return await context.VwSalaryMatrix.FirstOrDefaultAsync(jo=>jo.Id==matrixId);
        }

        public async Task<List<VwSalaryMatrix>> GetMatrixList()
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            return await context.VwSalaryMatrix.AsNoTracking().ToListAsync();
        }

        public async Task<int> CreateMatrix(SalaryMatrix matrix, List<SalaryMatrixBand> salaryBands)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();

            matrix.CreatedAt = DateTime.Now;
            matrix.IsActive = true;
            matrix.ApprovalStatusId = JOSalaryMatrixStatus.PendingApproval;

            await context.SalaryMatrix.AddAsync(matrix);
            await context.SaveChangesAsync();

            foreach (var item in salaryBands)
                item.SalaryMatrixId = matrix.Id;

            await context.SalaryMatrixBand.AddRangeAsync(salaryBands);
            await context.SaveChangesAsync();

            return matrix.Id;
        }

        public async Task<int> UpdateMatrixEffectiveDate(int matrixId,
            DateTime effectiveTo,
            bool isActive,
            int modifiedBy)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();

            var entity = await context.SalaryMatrix.FindAsync(matrixId);

            if (entity == null)
                return 0;

            entity.IsActive = isActive;

            if (isActive)
            {
                entity.EffectiveTo = effectiveTo;
                entity.ModifiedBy = modifiedBy;
                entity.ModifiedAt = DateTime.Now;
            }

            return await context.SaveChangesAsync();
        }

        #region Job Family/Level/Position Maintenance

        public async Task<int> SaveJobFamily(JobFamilies jobFamily)
        {
            ArgumentNullException.ThrowIfNull(jobFamily);

            var name = jobFamily.JobFamilyName?.Trim();
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Job family name is required.", nameof(jobFamily));
            if (name.Length > 100)
                throw new ArgumentException("Job family name cannot exceed 100 characters.", nameof(jobFamily));

            await using var context = await _contextFactory.CreateDbContextAsync();

            if (await context.JobFamilies.AnyAsync(jo => jo.JobFamilyName != null &&
                jo.JobFamilyName.ToLower() == name.ToLower()))
                throw new InvalidOperationException("A job family with the same name already exists.");

            jobFamily.JobFamilyName = name;
            await context.JobFamilies.AddAsync(jobFamily);
            return await context.SaveChangesAsync();
        }

        public async Task<int> UpdateJobFamily(JobFamilies jobFamily)
        {
            ArgumentNullException.ThrowIfNull(jobFamily);

            var name = jobFamily.JobFamilyName?.Trim();
            if (jobFamily.Id <= 0)
                throw new ArgumentException("A valid job family is required.", nameof(jobFamily));
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Job family name is required.", nameof(jobFamily));
            if (name.Length > 100)
                throw new ArgumentException("Job family name cannot exceed 100 characters.", nameof(jobFamily));

            await using var context = await _contextFactory.CreateDbContextAsync();

            if (await context.JobFamilies.AnyAsync(jo => jo.Id != jobFamily.Id &&
                jo.JobFamilyName != null && jo.JobFamilyName.ToLower() == name.ToLower()))
                throw new InvalidOperationException("A job family with the same name already exists.");

            var entity = await context.JobFamilies.FindAsync(jobFamily.Id);
            if (entity == null)
                return 0;

            entity.JobFamilyName = name;
            return await context.SaveChangesAsync();
        }

        public async Task<List<JobFamilies>> GetJobFamilies()
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            return await context.JobFamilies
                .AsNoTracking()
                .OrderBy(jo => jo.JobFamilyName)
                .ToListAsync();
        }

        public async Task<int> SaveJobLevel(JobLevels jobLevel)
        {
            ArgumentNullException.ThrowIfNull(jobLevel);

            var name = jobLevel.JobLevelName?.Trim();
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Job level name is required.", nameof(jobLevel));
            if (name.Length > 100)
                throw new ArgumentException("Job level name cannot exceed 100 characters.", nameof(jobLevel));

            await using var context = await _contextFactory.CreateDbContextAsync();

            if (await context.JobLevels.AnyAsync(jo => jo.JobLevelName != null &&
                jo.JobLevelName.ToLower() == name.ToLower()))
                throw new InvalidOperationException("A job level with the same name already exists.");

            jobLevel.JobLevelName = name;
            await context.JobLevels.AddAsync(jobLevel);
            return await context.SaveChangesAsync();
        }

        public async Task<int> UpdateJobLevel(JobLevels jobLevel)
        {
            ArgumentNullException.ThrowIfNull(jobLevel);

            var name = jobLevel.JobLevelName?.Trim();
            if (jobLevel.Id <= 0)
                throw new ArgumentException("A valid job level is required.", nameof(jobLevel));
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Job level name is required.", nameof(jobLevel));
            if (name.Length > 100)
                throw new ArgumentException("Job level name cannot exceed 100 characters.", nameof(jobLevel));

            await using var context = await _contextFactory.CreateDbContextAsync();

            if (await context.JobLevels.AnyAsync(jo => jo.Id != jobLevel.Id &&
                jo.JobLevelName != null && jo.JobLevelName.ToLower() == name.ToLower()))
                throw new InvalidOperationException("A job level with the same name already exists.");

            var entity = await context.JobLevels.FindAsync(jobLevel.Id);
            if (entity == null)
                return 0;

            entity.JobLevelName = name;
            return await context.SaveChangesAsync();
        }

        public async Task<List<JobLevels>> GetJobLevels()
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            return await context.JobLevels
                .AsNoTracking()
                .OrderBy(jo => jo.JobLevelName)
                .ToListAsync();
        }

        public async Task<int> SaveJobPosition(JobPositions jobPosition)
        {
            ArgumentNullException.ThrowIfNull(jobPosition);

            var name = jobPosition.PositionName?.Trim();
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Job position name is required.", nameof(jobPosition));
            if (name.Length > 50)
                throw new ArgumentException("Job position name cannot exceed 50 characters.", nameof(jobPosition));
            if (jobPosition.JobFamilyId.GetValueOrDefault() <= 0)
                throw new ArgumentException("Job family is required.", nameof(jobPosition));

            await using var context = await _contextFactory.CreateDbContextAsync();

            if (!await context.JobFamilies.AnyAsync(jo => jo.Id == jobPosition.JobFamilyId))
                throw new InvalidOperationException("The selected job family no longer exists.");

            if (await context.JobPositions.AnyAsync(jo => jo.JobFamilyId == jobPosition.JobFamilyId &&
                jo.PositionName != null && jo.PositionName.ToLower() == name.ToLower()))
                throw new InvalidOperationException("A job position with the same name already exists in this family.");

            jobPosition.PositionName = name;
            await context.JobPositions.AddAsync(jobPosition);
            return await context.SaveChangesAsync();
        }

        public async Task<int> UpdateJobPosition(JobPositions jobPosition)
        {
            ArgumentNullException.ThrowIfNull(jobPosition);

            var name = jobPosition.PositionName?.Trim();
            if (jobPosition.Id <= 0)
                throw new ArgumentException("A valid job position is required.", nameof(jobPosition));
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Job position name is required.", nameof(jobPosition));
            if (name.Length > 50)
                throw new ArgumentException("Job position name cannot exceed 50 characters.", nameof(jobPosition));
            if (jobPosition.JobFamilyId.GetValueOrDefault() <= 0)
                throw new ArgumentException("Job family is required.", nameof(jobPosition));

            await using var context = await _contextFactory.CreateDbContextAsync();

            if (!await context.JobFamilies.AnyAsync(jo => jo.Id == jobPosition.JobFamilyId))
                throw new InvalidOperationException("The selected job family no longer exists.");

            if (await context.JobPositions.AnyAsync(jo => jo.Id != jobPosition.Id &&
                jo.JobFamilyId == jobPosition.JobFamilyId && jo.PositionName != null &&
                jo.PositionName.ToLower() == name.ToLower()))
                throw new InvalidOperationException("A job position with the same name already exists in this family.");

            var entity = await context.JobPositions.FindAsync(jobPosition.Id);
            if (entity == null)
                return 0;

            entity.PositionName = name;
            entity.JobFamilyId = jobPosition.JobFamilyId;
            return await context.SaveChangesAsync();
        }

        public async Task<List<JobPositions>> GetJobPositions()
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            return await context.JobPositions
                .AsNoTracking()
                .OrderBy(jo => jo.PositionName)
                .ToListAsync();
        }

        #endregion
    }
}
