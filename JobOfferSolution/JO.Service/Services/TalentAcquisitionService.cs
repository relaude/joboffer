using JO.DataModel.Entity;
using JO.Persistence.DataAccess;
using JO.Service.Services.Contracts;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace JO.Service.Services
{
    public class TalentAcquisitionService : ITalentAcquisitionService
    {
        private readonly IDbContextFactory<JobOfferDbContext> _contextFactory;
        public TalentAcquisitionService(IDbContextFactory<JobOfferDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<int> CreatePackage(
            int transactionId,
            string packageName,
            decimal packageAmount,
            int priority)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();

            var joPackage = new JobOfferPackages
            {
                Transaction_Id = transactionId,
                PackageName = packageName,
                PackageAmount = packageAmount,
                Priority = priority,
                CreatedAt = DateTime.Now
            };

            await context.JobOfferPackages.AddAsync(joPackage);
            await context.SaveChangesAsync();

            return joPackage.Id;
        }

        public async Task<int> UpdatePackage(
            int id,
            string packageName,
            decimal packageAmount,
            int priority)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();

            var joPackage = context.JobOfferPackages.Find(id);
            joPackage.PackageName = packageName;
            joPackage.PackageAmount = packageAmount;
            joPackage.Priority = priority;

            context.JobOfferPackages.Update(joPackage);
            return await context.SaveChangesAsync();
        }
    }
}
