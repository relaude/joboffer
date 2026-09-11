using JO.BlazorDemoApp.Components.Pages.Candidate;
using JO.DataModel.DTOs;
using JO.DataModel.Entity;
using JO.DataModel.View;
using JO.Service.Constants;
using JO.Service.Extensions;
using JO.Service.Services.Contracts;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace JO.BlazorDemoApp.Components.Pages.TA.Candidate
{
    public partial class CandidateList
    {
        [Inject] private ICandidateService CandidateService { get; set; } = default!;
        private List<VwDboxCandidates> eligibleCandidates = new();
        private List<VwDboxCandidates> filteredCandidates = new();
        private PagedResult<VwDboxCandidates> pagedCandidates = new() { Page = 1, PageSize = 10 };
        private int total = 0; 
        private int withResponse = 0; 
        private int withOutResponse = 0; 
        private int joCreated = 0; 
        private int joDraft = 0; 
        private int forJOCreation = 0; 
        protected override async Task OnInitializedAsync()
        {
            eligibleCandidates = await CandidateService.GetTAPartnerDboxCandidates();

            FilterCandidates(null);

            SetKpiCount();
        }

        private void SetKpiCount()
        {
            total = eligibleCandidates.Count;
            withResponse = eligibleCandidates.Count(jo => jo.ResponseId > 0);
            withOutResponse = total - withResponse;

            forJOCreation = eligibleCandidates.Count(jo => jo.StatusId == 1 || jo.StatusId == 4);
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
                1 => eligibleCandidates
                    .Where(candidate => candidate.StatusId == statusId || candidate.StatusId == 4)
                    .ToList(),
                _ => eligibleCandidates
                    .Where(candidate => candidate.StatusId == statusId)
                    .ToList()
            };
            ChangePage(1);
        }

        private void ChangePage(int page) =>
            pagedCandidates = filteredCandidates.ToPagedResult(page, pagedCandidates.PageSize);

        private void ChangePageSize(int pageSize) =>
            pagedCandidates = filteredCandidates.ToPagedResult(1, pageSize);

    }
}
