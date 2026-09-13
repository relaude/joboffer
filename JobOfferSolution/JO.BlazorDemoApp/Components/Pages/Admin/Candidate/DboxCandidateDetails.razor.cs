using JO.DataModel.Entity;
using JO.Service.Services.Contracts;
using Microsoft.AspNetCore.Components;

namespace JO.BlazorDemoApp.Components.Pages.Admin.Candidate
{
    public partial class DboxCandidateDetails
    {
        [Inject] private IDBoxAPISyncService DBoxAPISyncService { get; set; } = default!;
        [Parameter] public int dboxCandidateId { get; set; }

        private DboxCandidatesRawData? candidate;
        private bool isLoading = true;
        private string? loadError;
        private int loadVersion;

        protected override Task OnParametersSetAsync() => LoadCandidateAsync();

        private async Task LoadCandidateAsync()
        {
            var version = ++loadVersion;
            var requestedId = dboxCandidateId;
            candidate = null;
            loadError = null;
            isLoading = true;
            try
            {
                if (requestedId > 0)
                {
                    var result = await DBoxAPISyncService.GetDboxCandidatesRawData(requestedId);
                    if (version == loadVersion)
                        candidate = result;
                }
            }
            catch (Exception)
            {
                if (version == loadVersion)
                    loadError = "Unable to load this DBox candidate. Please try again.";
            }
            finally
            {
                if (version == loadVersion)
                    isLoading = false;
            }
        }

        private static string DisplayValue(string? value) =>
            string.IsNullOrWhiteSpace(value) ? "-" : value;
    }
}
