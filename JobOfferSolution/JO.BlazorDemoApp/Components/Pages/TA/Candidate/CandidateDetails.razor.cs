using JO.DataModel.DTOs;
using JO.DataModel.Entity;
using JO.DataModel.View;
using JO.Service.Constants;
using JO.Service.Services.Contracts;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace JO.BlazorDemoApp.Components.Pages.TA.Candidate
{
    public partial class CandidateDetails
    {
        [Inject] private ICandidateService CandidateService { get; set; } = default!;
        [Inject] private IUtilitiesService UtilitiesService { get; set; } = default!;
        [Inject] private IAlertService AlertService { get; set; } = default!;
        [Inject] private IAccountService AccountService { get; set; } = default!;
        [Inject] private ICompensationService CompensationService { get; set; } = default!;
        [Inject] private NavigationManager Navigation { get; set; } = default!;

        [Parameter] public int candidateId { get; set; }
        private VwDboxCandidates candidate = new();

        protected override async Task OnInitializedAsync()
        {
            candidate = await CandidateService.GetVwDboxCandidate(candidateId);
        }

        private async Task CreateJobOffer()
        {
            int numProposal = await AlertService.ConfirmProposalNumber();

            if (numProposal == 0) return;

            int createdBy = await AccountService.GetJobOfferUserId();
            int jobOfferId = await CandidateService.CreateJobOffer(candidate, numProposal, createdBy);

            Navigation.NavigateTo($"{JORoutes.TA.Analysis}/{jobOfferId}");
        }

    }
}
