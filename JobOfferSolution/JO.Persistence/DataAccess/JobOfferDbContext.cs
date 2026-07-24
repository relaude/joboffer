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
        public DbSet<Permissions> Permissions { get; set; }
        public DbSet<UserDivisionAccess> UserDivisionAccess { get; set; }
        public DbSet<UserPermissions> UserPermissions { get; set; }

        //Job Offers
        public DbSet<Candidates> Candidates { get; set; }
        public DbSet<JobPositions> JobPositions { get; set; }
        public DbSet<Companies> Companies { get; set; }
        public DbSet<Divisions> Divisions { get; set; }
        public DbSet<Departments> Departments { get; set; }
        public DbSet<Requests> Requests { get; set; }
        public DbSet<JobOffers> JobOffers { get; set; }

        //Legal
        public DbSet<LegalEntities> LegalEntities { get; set; }

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

        //Analysis
        public DbSet<SalaryBandStatus> SalaryBandStatus { get; set; }
        public DbSet<CompBenPackages> CompBenPackages { get; set; }
        public DbSet<Proposal> Proposal { get; set; }

        //Approval
        public DbSet<Approvals> Approvals { get; set; }

        //Discussion
        public DbSet<Discussions> Discussions { get; set; }
        public DbSet<ChannelTypes> ChannelTypes { get; set; }
        public DbSet<DiscussSteps> DiscussSteps { get; set; }
        public DbSet<CandResponse> CandResponse { get; set; }

        //WorkFlow
        public DbSet<WorkFlow> WorkFlow { get; set; }
        public DbSet<WorkFlowStatus> WorkFlowStatus { get; set; }

        //Views
        public DbSet<VwApprovals> VwApprovals { get; set; }
        public DbSet<VwCompensationBenefits> VwCompensationBenefits { get; set; }
        public DbSet<VwDiscussions> VwDiscussions { get; set; }
        public DbSet<VwDivisions> VwDivisions { get; set; }
        public DbSet<VwJobOffers> VwJobOffers { get; set; }
        public DbSet<VwJobOfferWorkFlow> VwJobOfferWorkFlow { get; set; }
        public DbSet<VwJOUserRoles> VwJOUserRoles { get; set; }
        public DbSet<VwJOUsersInRoles> VwJOUsersInRoles { get; set; }
        public DbSet<VwLegalEntities> VwLegalEntities { get; set; }
        public DbSet<VwRolePermissions> VwRolePermissions { get; set; }
        public DbSet<VwSalaryMatrix> VwSalaryMatrix { get; set; }
        public DbSet<VwSalaryMatrixBand> VwSalaryMatrixBand { get; set; }
        public DbSet<VwUserDivisionAccess> VwUserDivisionAccess { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Views
            modelBuilder.Entity<VwApprovals>().HasNoKey().ToView("vw_Approvals");
            modelBuilder.Entity<VwCompensationBenefits>().HasNoKey().ToView("vw_CompensationBenefits");
            modelBuilder.Entity<VwDiscussions>().HasNoKey().ToView("vw_Discussions");
            modelBuilder.Entity<VwDivisions>().HasNoKey().ToView("vw_Divisions");
            modelBuilder.Entity<VwJobOffers>().HasNoKey().ToView("vw_JobOffers");
            modelBuilder.Entity<VwJobOfferWorkFlow>().HasNoKey().ToView("vw_JobOfferWorkFlow");
            modelBuilder.Entity<VwJOUserRoles>().HasNoKey().ToView("vw_JOUserRoles");
            modelBuilder.Entity<VwJOUsersInRoles>().HasNoKey().ToView("vw_JOUsersInRoles");
            modelBuilder.Entity<VwLegalEntities>().HasNoKey().ToView("vw_LegalEntities");
            modelBuilder.Entity<VwRolePermissions>().HasNoKey().ToView("vw_RolePermissions");
            modelBuilder.Entity<VwSalaryMatrix>().HasNoKey().ToView("vw_SalaryMatrix");
            modelBuilder.Entity<VwSalaryMatrixBand>().HasNoKey().ToView("vw_SalaryMatrixBand");
            modelBuilder.Entity<VwUserDivisionAccess>().HasNoKey().ToView("vw_UserDivisionAccess");
        }
    }
}
