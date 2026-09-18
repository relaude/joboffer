using JO.DataModel.DTOs;
using JO.DataModel.Entity;
using JO.DataModel.View;
using JO.Service.Extensions;
using JO.Service.Constants;
using JO.Service.Services.Contracts;
using Microsoft.AspNetCore.Components;

namespace JO.BlazorDemoApp.Components.Pages.TA.JobOfferEmail
{
    public partial class JobOfferEmailList
    {
        [Inject] private IJOLetterService JOLetterService { get; set; } = default!;
        [Inject] private ILogger<JobOfferEmailList> Logger { get; set; } = default!;
        [Inject] private NavigationManager Navigation { get; set; } = default!;

        private List<VwJobOfferHasEmail> emails = new();
        private List<VwJobOfferHasEmail> filteredEmails = new();
        private List<JOHasEmailStatus> statuses = new();
        private PagedResult<VwJobOfferHasEmail> pagedEmails = new() { Page = 1, PageSize = 10 };
        private int? statusId;
        private string joRefNumSearch = string.Empty;
        private string candidateNameSearch = string.Empty;
        private bool isLoading = true;
        private string? loadError;
        private Shared.JOModal? newJOEmailModal;
        private List<JobOffers> jobOffersForDiscussion = new();
        private int? selectedJobOfferId;
        private bool isLoadingJobOffers;
        private string? jobOffersLoadError;

        protected override Task OnInitializedAsync() => LoadEmailsAsync();

        private void ContinueToNewJOEmail()
        {
            if (isLoadingJobOffers || jobOffersLoadError is not null || selectedJobOfferId is not int jobOfferId)
                return;

            Navigation.NavigateTo($"{JORoutes.TAPartner.JobOfferNewEmail}/{jobOfferId}");
        }

        private async Task ShowNewJOEmailAsync()
        {
            if (isLoadingJobOffers)
                return;

            selectedJobOfferId = null;
            jobOffersLoadError = null;
            jobOffersForDiscussion.Clear();
            isLoadingJobOffers = true;
            newJOEmailModal?.Show();
            try
            {
                jobOffersForDiscussion = await JOLetterService.GetJobOffersForDiscussion();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to load job offers for discussion.");
                jobOffersLoadError = "Unable to load job offers for discussion. Please try again.";
            }
            finally
            {
                isLoadingJobOffers = false;
            }
        }

        private async Task LoadEmailsAsync()
        {
            isLoading = true;
            loadError = null;
            emails.Clear();
            filteredEmails.Clear();
            ChangePage(1);
            try
            {
                statuses = await JOLetterService.GetJOHasEmailStatus();
                emails = (await JOLetterService.GetVwJobOfferHasEmail())
                    .OrderByDescending(email => email.CreatedAt)
                    .ThenByDescending(email => email.Id)
                    .ToList();
                SearchEmails();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to load job offer emails.");
                loadError = "Unable to load job offer emails. Please try again.";
            }
            finally
            {
                isLoading = false;
            }
        }

        private void SearchEmails()
        {
            var reference = joRefNumSearch.Trim();
            var name = candidateNameSearch.Trim();
            filteredEmails = emails.Where(email =>
                (!statusId.HasValue || email.StatusId == statusId) &&
                MatchesPartial(email.JORefNum, reference) &&
                MatchesPartial(email.CandidateName, name)).ToList();
            ChangePage(1);
        }

        private void ClearSearch()
        {
            statusId = null;
            joRefNumSearch = candidateNameSearch = string.Empty;
            SearchEmails();
        }

        private static string GetEmailUrl(VwJobOfferHasEmail email) =>
            $"{(email.StatusId == 3 ? JORoutes.TAPartner.JobOfferSendEmail : JORoutes.TAPartner.JobOfferEmail)}/{email.Id}";

        private void ChangePage(int page) =>
            pagedEmails = filteredEmails.ToPagedResult(page, pagedEmails.PageSize);

        private void ChangePageSize(int pageSize) =>
            pagedEmails = filteredEmails.ToPagedResult(1, pageSize);

        // Match ResponseRawData: case-insensitive, in-memory LIKE '%term%' matching.
        private static bool MatchesPartial(string? value, string term) =>
            term.Length == 0 || (value?.Contains(term, StringComparison.OrdinalIgnoreCase) ?? false);

        private static string DisplayValue(string? value) => string.IsNullOrWhiteSpace(value) ? "-" : value;
        private static string DisplayDate(DateTime? value) => value?.ToString("dd MMM yyyy, HH:mm") ?? "-";
    }
}
