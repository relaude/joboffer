using JO.DataModel.Entity;
using JO.Service.Services.Contracts;
using Microsoft.AspNetCore.Components;

namespace JO.BlazorDemoApp.Components.Pages.Letter
{
    public partial class JOEmailDetails
    {
        [Inject] private IJOLetterService JOLetterService { get; set; } = default!;
        [Inject] private ILogger<JOEmailDetails> Logger { get; set; } = default!;

        [Parameter, EditorRequired] public int EmailId { get; set; }

        private readonly string componentId = $"jo-email-details-{Guid.NewGuid():N}";
        private JobOfferHasEmail jobOfferEmail = new();
        private List<JOHasEmailAttach> attachments = new();
        private bool isLoading = true;
        private string? loadError;

        protected override async Task OnParametersSetAsync()
        {
            isLoading = true;
            loadError = null;
            jobOfferEmail = new();
            attachments.Clear();
            try
            {
                var email = await JOLetterService.GetJobOfferHasEmail(EmailId);
                if (email is null)
                {
                    loadError = "This email is not available.";
                    return;
                }

                jobOfferEmail = email;
                attachments = await JOLetterService.GetJOHasEmailAttach(EmailId);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to load email {EmailId} details.", EmailId);
                loadError = "Unable to load this email. Please reload the page and try again.";
            }
            finally
            {
                isLoading = false;
            }
        }
    }
}
