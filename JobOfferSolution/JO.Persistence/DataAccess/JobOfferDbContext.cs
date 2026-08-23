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

        //Candidates
        public DbSet<CandidateResponses> CandidateResponses { get; set; }
        public DbSet<DboxCandidates> DboxCandidates { get; set; }

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
        public DbSet<SalaryBands> SalaryBands { get; set; }
        public DbSet<Currencies> Currencies { get; set; }
        public DbSet<JobLevels> JobLevels { get; set; }
        public DbSet<JobFamilies> JobFamilies { get; set; }
        public DbSet<JobPositionGrades> JobPositionGrades { get; set; }
        public DbSet<SalaryGrades> SalaryGrades { get; set; }

        //Mass Upload
        public DbSet<CandidateExcelRawData> CandidateExcelRawData { get; set; }
        public DbSet<CandidateTempData> CandidateTempData { get; set; }
        public DbSet<CandidateMassUploadLogs> CandidateMassUploadLogs { get; set; }

        //Analysis
        public DbSet<SalaryBandStatus> SalaryBandStatus { get; set; }
        public DbSet<Proposal> Proposal { get; set; }

        //Compensation
        public DbSet<CompBenPackages> CompBenPackages { get; set; }
        public DbSet<CompBenItems> CompBenItems { get; set; }
        public DbSet<CompBenTypes> CompBenTypes { get; set; }
        public DbSet<CompensationBenefits> CompensationBenefits { get; set; }
        public DbSet<Frequencies> Frequencies { get; set; }
        public DbSet<CompBenEmpType> CompBenEmpType { get; set; }
        public DbSet<CompBenPlans> CompBenPlans { get; set; }
        public DbSet<CompBenArea> CompBenArea { get; set; }
        public DbSet<CompBenSched> CompBenSched { get; set; }
        public DbSet<CompBenShift> CompBenShift { get; set; }
        public DbSet<CompBenClass> CompBenClass { get; set; }
        public DbSet<CompBenFreq> CompBenFreq { get; set; }
        public DbSet<CompBenMRI> CompBenMRI { get; set; }
        public DbSet<CompBenItmCat> CompBenItmCat { get; set; }
        public DbSet<CompBenPckgs> CompBenPckgs { get; set; }
        public DbSet<CBPlnHasItem> CBPlnHasItem { get; set; }
        public DbSet<CBPckHasItem> CBPckHasItem { get; set; }
        public DbSet<PckgTemp> PckgTemp { get; set; }
        public DbSet<PckgTempHasItms> PckgTempHasItms { get; set; }
        public DbSet<PckgItems> PckgItems { get; set; }
        public DbSet<CompensationPackage> CompensationPackage { get; set; }
        public DbSet<CompensationOptions> CompensationOptions { get; set; }
        public DbSet<CompensationItem> CompensationItem { get; set; }
        public DbSet<CompensationItems> CompensationItems { get; set; }
        public DbSet<CompenItemCategory> CompenItemCategory { get; set; }
        public DbSet<CompensationTemplate> CompensationTemplate { get; set; }
        public DbSet<CompensationTemplateItems> CompensationTemplateItems { get; set; }
        public DbSet<CompanyCompensation> CompanyCompensation { get; set; }
        public DbSet<CompanyCompensationItems> CompanyCompensationItems { get; set; }
        public DbSet<JOCompanyCompensation> JOCompanyCompensation { get; set; }
        public DbSet<JOCompanyCompensationItems> JOCompanyCompensationItems { get; set; }

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
        public DbSet<VwCompanySalaryGrades> VwCompanySalaryGrades { get; set; }
        public DbSet<VwCompensationBenefits> VwCompensationBenefits { get; set; }
        public DbSet<VwDiscussions> VwDiscussions { get; set; }
        public DbSet<VwDivisions> VwDivisions { get; set; }
        public DbSet<VwJobOffers> VwJobOffers { get; set; }
        public DbSet<VwJobOfferWorkFlow> VwJobOfferWorkFlow { get; set; }
        public DbSet<VwJOUserRoles> VwJOUserRoles { get; set; }
        public DbSet<VwJOUsersInRoles> VwJOUsersInRoles { get; set; }
        public DbSet<VwLegalEntities> VwLegalEntities { get; set; }
        public DbSet<VwRolePermissions> VwRolePermissions { get; set; }
        public DbSet<VwSalaryBands> VwSalaryBands { get; set; }
        public DbSet<VwSalaryMatrix> VwSalaryMatrix { get; set; }
        public DbSet<VwSalaryMatrixBand> VwSalaryMatrixBand { get; set; }
        public DbSet<VwUserDivisionAccess> VwUserDivisionAccess { get; set; }
        public DbSet<VwCompBenItems> VwCompBenItems { get; set; }
        public DbSet<VwCompBenPlans> VwCompBenPlans { get; set; }
        public DbSet<VwPckgTempHasItms> VwPckgTempHasItms { get; set; }
        public DbSet<VwPckgTemp> VwPckgTemp { get; set; }
        public DbSet<VwDboxCandidates> VwDboxCandidates { get; set; }
        public DbSet<VwJODboxCandidates> VwJODboxCandidates { get; set; }
        public DbSet<VwCompensationTemplateItems> VwCompensationTemplateItems { get; set; }
        public DbSet<VwJOUserAspNetRoles> VwJOUserAspNetRoles { get; set; }
        public DbSet<VwJobOfferUsers> VwJobOfferUsers { get; set; }
        public DbSet<VwJobOfferUsersAndRoles> VwJobOfferUsersAndRoles { get; set; }
        public DbSet<VwCompanyCompensation> VwCompanyCompensation { get; set; }
        public DbSet<VwCompanyCompensationItems> VwCompanyCompensationItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Views
            modelBuilder.Entity<VwApprovals>().HasNoKey().ToView("vw_Approvals");
            modelBuilder.Entity<VwCompanySalaryGrades>().HasNoKey().ToView("vw_CompanySalaryGrades");
            modelBuilder.Entity<VwCompensationBenefits>().HasNoKey().ToView("vw_CompensationBenefits");
            modelBuilder.Entity<VwDiscussions>().HasNoKey().ToView("vw_Discussions");
            modelBuilder.Entity<VwDivisions>().HasNoKey().ToView("vw_Divisions");
            modelBuilder.Entity<VwJobOffers>().HasNoKey().ToView("vw_JobOffers");
            modelBuilder.Entity<VwJobOfferWorkFlow>().HasNoKey().ToView("vw_JobOfferWorkFlow");
            modelBuilder.Entity<VwJOUserRoles>().HasNoKey().ToView("vw_JOUserRoles");
            modelBuilder.Entity<VwJOUsersInRoles>().HasNoKey().ToView("vw_JOUsersInRoles");
            modelBuilder.Entity<VwLegalEntities>().HasNoKey().ToView("vw_LegalEntities");
            modelBuilder.Entity<VwRolePermissions>().HasNoKey().ToView("vw_RolePermissions");
            modelBuilder.Entity<VwSalaryBands>().HasNoKey().ToView("vw_SalaryBands");
            modelBuilder.Entity<VwSalaryMatrix>().HasNoKey().ToView("vw_SalaryMatrix");
            modelBuilder.Entity<VwSalaryMatrixBand>().HasNoKey().ToView("vw_SalaryMatrixBand");
            modelBuilder.Entity<VwUserDivisionAccess>().HasNoKey().ToView("vw_UserDivisionAccess");
            modelBuilder.Entity<VwCompBenItems>().HasNoKey().ToView("vw_CompBenItems");
            modelBuilder.Entity<VwCompBenPlans>().HasNoKey().ToView("vw_CompBenPlans");
            modelBuilder.Entity<VwPckgTempHasItms>().HasNoKey().ToView("vw_PckgTempHasItms");
            modelBuilder.Entity<VwPckgTemp>().HasNoKey().ToView("vw_PckgTemp");
            modelBuilder.Entity<VwDboxCandidates>().HasNoKey().ToView("vw_DboxCandidates");
            modelBuilder.Entity<VwJODboxCandidates>().HasNoKey().ToView("vw_JODboxCandidates");
            modelBuilder.Entity<VwCompensationTemplateItems>().HasNoKey().ToView("vw_CompensationTemplateItems");
            modelBuilder.Entity<VwJOUserAspNetRoles>().HasNoKey().ToView("vw_JOUserAspNetRoles");
            modelBuilder.Entity<VwJobOfferUsers>().HasNoKey().ToView("vw_JobOfferUsers");
            modelBuilder.Entity<VwJobOfferUsersAndRoles>().HasNoKey().ToView("vw_JobOfferUsersAndRoles");
            modelBuilder.Entity<VwCompanyCompensation>().HasNoKey().ToView("vw_CompanyCompensation");
            modelBuilder.Entity<VwCompanyCompensationItems>().HasNoKey().ToView("vw_CompanyCompensationItems");
        }
    }
}
