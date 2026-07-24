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
    }
}
