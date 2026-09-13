using JO.DataModel.View;
using JO.Service.Services.Contracts;
using Microsoft.AspNetCore.Components;

namespace JO.BlazorDemoApp.Components.Pages.Admin.Candidate
{
    public partial class CandidateDetails
    {
        [Inject] private ICandidateService CandidateService { get; set; } = default!;
        [Parameter] public int candidateId { get; set; }

        private VwDboxCandidates? candidate;
        private bool isLoading = true;
        private string? loadError;
        private int loadVersion;

        protected override Task OnParametersSetAsync() => LoadCandidateAsync();

        private async Task LoadCandidateAsync()
        {
            var version = ++loadVersion;
            var requestedId = candidateId;
            candidate = null;
            loadError = null;
            isLoading = true;
            try
            {
                if (requestedId > 0)
                {
                    var result = await CandidateService.GetVwDboxCandidate(requestedId);
                    if (version == loadVersion)
                        candidate = result;
                }
            }
            catch (Exception)
            {
                if (version == loadVersion)
                    loadError = "Unable to load this candidate. Please try again.";
            }
            finally
            {
                if (version == loadVersion)
                    isLoading = false;
            }
        }
    }
}
