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

        private int countAnalysis = 0;
        private int countForReview = 0;
        private int countReviwed = 0;
        private int countApproved = 0;

        protected override async Task OnInitializedAsync()
        {
            joDboxCandidates = await JODetailsService.GetVwJODboxCandidates();
            SetUpCountStatus();
        }

        private void SetUpCountStatus()
        {
            int?[] approvedStatusIds = { 4,5,6,7,8 };

            countAnalysis = joDboxCandidates.Where(jo => jo.StatusId == 1).Count();//Analysis
            countForReview = joDboxCandidates.Where(jo => jo.StatusId == 2).Count();//For Review
            countReviwed = joDboxCandidates.Where(jo => jo.StatusId == 3).Count();//Reviewed
            countApproved = joDboxCandidates.Where(jo => approvedStatusIds.Contains(jo.StatusId)).Count();//Approved
        }
    }
}
