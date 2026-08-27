using JO.DataModel.DTOs;
using JO.DataModel.Entity;
using JO.DataModel.View;
using JO.Service.Constants;
using JO.Service.Services.Contracts;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace JO.BlazorDemoApp.Components.Pages.TA.JobOffer
{
    public partial class JobOfferList
    {
        [Inject] private IJODetailsService JODetailsService { get; set; } = default!;
        [Inject] private NavigationManager Navigation { get; set; } = default!;

        private List<VwJODboxCandidates> joDboxCandidates = new();
        private List<VwJODboxCandidates> filteredJODboxCandidates = new();

        private int total = 0;
        private int countForReview = 0;
        private int countForApproval = 0;
        private int countApproved = 0;

        protected override async Task OnInitializedAsync()
        {
            joDboxCandidates = await JODetailsService.GetVwJODboxCandidates();
            filteredJODboxCandidates = joDboxCandidates.Where(jo => jo.WorkFlowId > 1).ToList();
            SetUpCountStatus();
        }

        private void SetUpCountStatus()
        {
            int?[] forApprovalIds = { 4,5,6,7 };

            total = filteredJODboxCandidates.Count();
            countForReview = filteredJODboxCandidates.Where(jo => jo.WorkFlowId == 3).Count();//For Review
            countApproved = filteredJODboxCandidates.Where(jo => jo.WorkFlowId == 8).Count();//For Discussion
            countForApproval = filteredJODboxCandidates.Where(jo => forApprovalIds.Contains(jo.WorkFlowId)).Count();//For Approval
        }

        private string SetJOlink(VwJODboxCandidates joDboxCandidate)
        {
            if(joDboxCandidate.WorkFlowId == 2)//Created
            {
                return $"{JORoutes.TA.Analysis}/{joDboxCandidate.Id}";
            }

            return $"{JORoutes.TA.JobOfferDetails}/{joDboxCandidate.Id}";
        }
    }
}
