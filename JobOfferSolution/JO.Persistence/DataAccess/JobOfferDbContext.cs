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
        public DbSet<JobPositions> JobPositions { get; set; }
        public DbSet<Departments> Departments { get; set; }
        public DbSet<JobOffers> JobOffers { get; set; }
        public DbSet<MainStatus> MainStatus { get; set; }
        public DbSet<CandidateStatus> CandidateStatus { get; set; }
        public DbSet<DeclineReasons> DeclineReasons { get; set; }
        public DbSet<ReturnReasons> ReturnReasons { get; set; }

        //Views
        public DbSet<VwCandidates> VwCandidates { get; set; }
        public DbSet<VwJobOffers> VwJobOffers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<VwCandidates>().HasNoKey().ToView("vw_Candidates");
            modelBuilder.Entity<VwJobOffers>().HasNoKey().ToView("vw_JobOffers");
        }
    }
}
