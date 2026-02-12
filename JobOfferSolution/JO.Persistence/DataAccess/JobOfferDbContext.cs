using JO.DataModel.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace JO.Persistence.DataAccess
{
    public class JobOfferDbContext : DbContext
    {
        public JobOfferDbContext(DbContextOptions<JobOfferDbContext> options) : base(options) { }

        public DbSet<JobOfferUsers> JobOfferUsers { get; set; }
    }
}
