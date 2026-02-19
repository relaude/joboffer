using JO.DataModel.Entity;
using JO.DataModel.View;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace JO.Persistence.DataAccess
{
    public class JobOfferDbContext : DbContext
    {
        public JobOfferDbContext(DbContextOptions<JobOfferDbContext> options) : base(options) { }

        //Tables
        public DbSet<JobOfferUsers> JobOfferUsers { get; set; }
        public DbSet<Candidates> Candidates { get; set; }
        public DbSet<JobOfferTransactions> JobOfferTransactions { get; set; }
        public DbSet<TransactionAttachments> TransactionAttachments { get; set; }
        public DbSet<JobOfferPackages> JobOfferPackages { get; set; }

        //Views
        public DbSet<VwJobOfferTransactions> VwJobOfferTransactions { get; set; }
        public DbSet<VwTransactionAttachments> VwTransactionAttachments { get; set; }
        public DbSet<VwCandidates> VwCandidates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<VwJobOfferTransactions>().HasNoKey().ToView("vw_JobOfferTransactions");
            modelBuilder.Entity<VwTransactionAttachments>().HasNoKey().ToView("vw_TransactionAttachments");
            modelBuilder.Entity<VwCandidates>().HasNoKey().ToView("vw_Candidates");
        }
    }
}
