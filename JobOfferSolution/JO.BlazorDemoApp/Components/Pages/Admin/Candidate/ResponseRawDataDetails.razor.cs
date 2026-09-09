using JO.DataModel.Entity;
using JO.Service.Services.Contracts;
using Microsoft.AspNetCore.Components;

namespace JO.BlazorDemoApp.Components.Pages.Admin.Candidate
{
    public partial class ResponseRawDataDetails
    {
        [Inject] private ICandidateService CandidateService { get; set; } = default!;
        [Parameter] public int rawDataId { get; set; }

        private CandidateResponseRawData? response;
        private bool isLoading = true;
        private string? loadError;
        private int loadVersion;

        protected override Task OnParametersSetAsync() => LoadResponseAsync();

        private async Task LoadResponseAsync()
        {
            var version = ++loadVersion;
            var requestedId = rawDataId;
            response = null;
            loadError = null;
            isLoading = true;
            try
            {
                if (requestedId > 0)
                {
                    var result = await CandidateService.GetCandidateResponsesRawData(requestedId);
                    if (version == loadVersion)
                        response = result;
                }
            }
            catch (Exception)
            {
                if (version == loadVersion)
                    loadError = "Unable to load this candidate response. Please try again.";
            }
            finally
            {
                if (version == loadVersion)
                    isLoading = false;
            }
        }

        private string ValidationStatus => response?.InvalidResponse switch
        {
            true => "Needs review",
            false => "Valid",
            _ => "Not validated"
        };

        private string ValidationBadge => response?.InvalidResponse switch
        {
            true => "badge-warning",
            false => "badge-success",
            _ => "badge-secondary"
        };

        private static string DisplayValue(string? value) =>
            string.IsNullOrWhiteSpace(value) ? "-" : value;
    }
}
