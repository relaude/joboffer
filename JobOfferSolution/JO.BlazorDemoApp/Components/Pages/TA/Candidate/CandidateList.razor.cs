using JO.DataModel.DTOs;
using JO.DataModel.Entity;
using JO.DataModel.View;
using JO.Service.Constants;
using JO.Service.Services.Contracts;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace JO.BlazorDemoApp.Components.Pages.TA.Candidate
{
    public partial class CandidateList
    {
        [Inject] private ICandidateService CandidateService { get; set; } = default!;
        [Inject] private IUtilitiesService UtilitiesService { get; set; } = default!;
        [Inject] private IAlertService AlertService { get; set; } = default!;
        [Inject] private IAccountService AccountService { get; set; } = default!;
        [Inject] private NavigationManager Navigation { get; set; } = default!;

        private List<VwDboxCandidates> candidates = new();
        private List<VwDboxCandidates> filteredCandidates = new();

        private int?[] AMSG = { 1, 109 };
        private int total = 0; 
        private int withResponse = 0; 
        private int withOutResponse = 0; 
        private int joCreated = 0; 
        private int joDraft = 0; 
        private int forJOCreation = 0; 
        protected override async Task OnInitializedAsync()
        {
            candidates = await CandidateService.GetVwDboxCandidates();

            filteredCandidates = candidates
                .Where(jo => !AMSG.Contains(jo.CSGId)
                    && jo.DivisionId != 3)
                .ToList();

            SetKpiCount();
        }

        private void SetKpiCount()
        {
            total = filteredCandidates.Count();
            withResponse = filteredCandidates.Where(jo => jo.ResponseId > 0).Count();
            withOutResponse = total - withResponse;

            forJOCreation = filteredCandidates.Where(jo => jo.StatusId==1).Count();
            joDraft = filteredCandidates.Where(jo => jo.StatusId==2).Count();
            joCreated = filteredCandidates.Where(jo => jo.StatusId==3).Count();
        }

        private async Task OpenCandidate(VwDboxCandidates candidate)
        {
            var candidateLink = await CandidateService.GetCandidateLink(candidate);
            Navigation.NavigateTo(candidateLink);
        }
    }
}
