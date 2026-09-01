using JO.DataModel.DTOs;
using JO.DataModel.Entity;
using JO.DataModel.View;
using JO.Service.Services.Contracts;
using Microsoft.AspNetCore.Components;

namespace JO.BlazorDemoApp.Components.Pages.TALead.JobOffer
{
    public partial class JobOfferDetailsLog
    {
        [Inject] private IDiscussionService DiscussionService { get; set; } = default!;
        [Inject] private ICompensationService CompensationService { get; set; } = default!;

        [Parameter] public int jobOfferId { get; set; }

        private TabName activeTab = TabName.JODetails;
        private JobOffers jobOffer = new();
        private VwJODboxCandidates vwJODboxCandidates = new();
        private VwDboxCandidates candidate = new();
        private VwSalaryBands vwSalaryBand = new();
        private JOAnalysis joAnalysis = new();
        private List<VwCompanyCompensationItems> vwCompanyCompensationItems = [];
        private List<CompanyCompensation> companyCompensation = [];
        private List<JOCompanyCompensation> joCompanyCompensation = [];
        private List<JOCompanyCompensationItems> joCompanyCompensationItems = [];
        private List<CompenItemCategoryDto> compenItemCategoryDto = [];
        private List<VwDiscussions> vwDiscussions = [];

        protected override async Task OnParametersSetAsync()
        {
            jobOffer = await DiscussionService.GetJobOffer(jobOfferId);
            vwJODboxCandidates = await DiscussionService.GetVwJODboxCandidates(jobOfferId);
            candidate = await DiscussionService.GetVwDboxCandidate(jobOffer.CandidateId.GetValueOrDefault());
            vwCompanyCompensationItems = await CompensationService.GetVwCompanyCompensationItems(
                jobOffer.CmpnyCmpnstnId.GetValueOrDefault());
            vwSalaryBand = await CompensationService.GetVwSalaryBand(
                jobOffer.CompanyId.GetValueOrDefault(), candidate.CSGId.GetValueOrDefault());
            joAnalysis = await CompensationService.GetJOAnalysis(jobOfferId);
            companyCompensation = await CompensationService.GetCompanyCompensation(
                jobOffer.CompanyId.GetValueOrDefault());
            joCompanyCompensation = await CompensationService.GetJOCompanyCompensation(jobOfferId);
            joCompanyCompensationItems = await CompensationService.GetJOCmpnyCompensationItems(jobOfferId);
            compenItemCategoryDto = await CompensationService.SetUpCompenItemCategoryDto();
            vwDiscussions = await DiscussionService.GetDiscussions(jobOfferId);
        }

        private void SelectTab(TabName tab)
        {
            activeTab = tab;
        }

        private enum TabName
        {
            JODetails,
            //Discussion,
            ActionLogs
        }
    }
}
