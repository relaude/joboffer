using JO.BlazorDemoApp.Components.Pages.Candidate;
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

        private List<VwDboxCandidates> candidates = new();
        private List<VwDboxCandidates> eligibleCandidates = new();
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

            eligibleCandidates = candidates
                .Where(jo => !AMSG.Contains(jo.CSGId)
                    && jo.DivisionId != 3)
                .ToList();

            filteredCandidates = eligibleCandidates.ToList();

            SetKpiCount();
        }

        private void SetKpiCount()
        {
            total = eligibleCandidates.Count;
            withResponse = eligibleCandidates.Count(jo => jo.ResponseId > 0);
            withOutResponse = total - withResponse;

            forJOCreation = eligibleCandidates.Count(jo => jo.StatusId == 1);
            joDraft = eligibleCandidates.Count(jo => jo.StatusId == 2);
            joCreated = eligibleCandidates.Count(jo => jo.StatusId == 3);
        }

        private void FilterCandidates(int? statusId)
        {
            filteredCandidates = statusId switch
            {
                null => eligibleCandidates.ToList(),
                CandidateKpiBoxes.WithResponseFilter => eligibleCandidates
                    .Where(candidate => candidate.ResponseId > 0)
                    .ToList(),
                CandidateKpiBoxes.WithoutResponseFilter => eligibleCandidates
                    .Where(candidate => candidate.ResponseId.GetValueOrDefault() <= 0)
                    .ToList(),
                _ => eligibleCandidates
                    .Where(candidate => candidate.StatusId == statusId)
                    .ToList()
            };
        }

    }
}
