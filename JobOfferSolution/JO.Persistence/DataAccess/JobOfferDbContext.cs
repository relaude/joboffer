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

        //RBAC+ABAC
        public DbSet<JobOfferUsers> JobOfferUsers { get; set; }
        public DbSet<UserAttributes> UserAttributes { get; set; }
        public DbSet<Permissions> Permissions { get; set; }
        public DbSet<UserApprovalLimits> UserApprovalLimits { get; set; }
        public DbSet<UserDivisionAccess> UserDivisionAccess { get; set; }
        public DbSet<UserPermissions> UserPermissions { get; set; }

        //Candidates
        public DbSet<Candidates> Candidates { get; set; }
        public DbSet<JobPositions> JobPositions { get; set; }
        public DbSet<Companies> Companies { get; set; }
        public DbSet<Divisions> Divisions { get; set; }
        public DbSet<Departments> Departments { get; set; }
        public DbSet<JobOffers> JobOffers { get; set; }

        //Company Setup
        public DbSet<CandidateApplications> CandidateApplications { get; set; }

        //Status
        public DbSet<MainStatus> MainStatus { get; set; }
        public DbSet<CandidateStatus> CandidateStatus { get; set; }

        //Logs
        public DbSet<DeclineReasons> DeclineReasons { get; set; }
        public DbSet<ReturnReasons> ReturnReasons { get; set; }
        public DbSet<ReturnLogs> ReturnLogs { get; set; }
        public DbSet<ActivityLogs> ActivityLogs { get; set; }

        //Salary
        public DbSet<SalaryMatrix> SalaryMatrix { get; set; }
        public DbSet<SalaryMatrixBand> SalaryMatrixBand { get; set; }
        public DbSet<Currencies> Currencies { get; set; }
        public DbSet<JobLevels> JobLevels { get; set; }
        public DbSet<JobFamilies> JobFamilies { get; set; }
        public DbSet<JobPositionGrades> JobPositionGrades { get; set; }

        //Mass Upload
        public DbSet<CandidateExcelRawData> CandidateExcelRawData { get; set; }
        public DbSet<CandidateTempData> CandidateTempData { get; set; }
        public DbSet<CandidateMassUploadLogs> CandidateMassUploadLogs { get; set; }

        //MS Forms
        public DbSet<CandidateMSFormRequests> CandidateMSFormRequests { get; set; }
        public DbSet<SubmittedDocuments> SubmittedDocuments { get; set; }

        //Analysis
        public DbSet<JobOfferProposal> JobOfferProposal { get; set; }

        //Views
        public DbSet<VwCandidates> VwCandidates { get; set; }
        public DbSet<VwJobOffers> VwJobOffers { get; set; }
        public DbSet<VwReturnLogs> VwReturnLogs { get; set; }
        public DbSet<VwActivityLogs> VwActivityLogs { get; set; }
        public DbSet<VwSalaryMatrix> VwSalaryMatrix { get; set; }
        public DbSet<VwSalaryMatrixBand> VwSalaryMatrixBand { get; set; }
        public DbSet<VwJOUserRoles> VwJOUserRoles { get; set; }
        public DbSet<VwDivisions> VwDivisions { get; set; }
        public DbSet<VwRolePermissions> VwRolePermissions { get; set; }
        public DbSet<VwJOUsersInRoles> VwJOUsersInRoles { get; set; }
        public DbSet<VwUserDivisionAccess> VwUserDivisionAccess { get; set; }
        public DbSet<VwCandidateMSFormRequests> VwCandidateMSFormRequests { get; set; }
        public DbSet<VwCandidateApplications> VwCandidateApplications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<VwCandidates>().HasNoKey().ToView("vw_Candidates");
            modelBuilder.Entity<VwJobOffers>().HasNoKey().ToView("vw_JobOffers");
            modelBuilder.Entity<VwReturnLogs>().HasNoKey().ToView("vw_ReturnLogs");
            modelBuilder.Entity<VwActivityLogs>().HasNoKey().ToView("vw_ActivityLogs");
            modelBuilder.Entity<VwSalaryMatrix>().HasNoKey().ToView("vw_SalaryMatrix");
            modelBuilder.Entity<VwSalaryMatrixBand>().HasNoKey().ToView("vw_SalaryMatrixBand");
            modelBuilder.Entity<VwJOUserRoles>().HasNoKey().ToView("vw_JOUserRoles");
            modelBuilder.Entity<VwDivisions>().HasNoKey().ToView("vw_Divisions");
            modelBuilder.Entity<VwRolePermissions>().HasNoKey().ToView("vw_RolePermissions");
            modelBuilder.Entity<VwJOUsersInRoles>().HasNoKey().ToView("vw_JOUsersInRoles");
            modelBuilder.Entity<VwUserDivisionAccess>().HasNoKey().ToView("vw_UserDivisionAccess");
            modelBuilder.Entity<VwCandidateMSFormRequests>().HasNoKey().ToView("vw_CandidateMSFormRequests");
            modelBuilder.Entity<VwCandidateApplications>().HasNoKey().ToView("vw_CandidateApplications");
        }
    }
}
