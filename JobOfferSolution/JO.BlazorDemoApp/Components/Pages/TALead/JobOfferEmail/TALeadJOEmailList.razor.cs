using JO.Service.Constants;
using JO.DataModel.DTOs;
using JO.DataModel.Entity;
using JO.DataModel.View;
using JO.Service.Extensions;

using JO.Service.Services.Contracts;
using Microsoft.AspNetCore.Components;

namespace JO.BlazorDemoApp.Components.Pages.TALead.JobOfferEmail
{
    public partial class TALeadJOEmailList
    {
        [Inject] private IJOLetterService JOLetterService { get; set; } = default!;
        [Inject] private ILogger<TALeadJOEmailList> Logger { get; set; } = default!;

        private List<VwJobOfferHasEmail> emails = new();
        private List<VwJobOfferHasEmail> filteredEmails = new();
        private List<JOHasEmailStatus> statuses = new();
        private PagedResult<VwJobOfferHasEmail> pagedEmails = new() { Page = 1, PageSize = 10 };
        private int statusId = 2;
        private string joRefNumSearch = string.Empty;
        private string candidateNameSearch = string.Empty;
        private bool isLoading = true;
        private string? loadError;
        protected override Task OnInitializedAsync() => LoadEmailsAsync();

        private async Task LoadEmailsAsync()
        {
            isLoading = true;
            loadError = null;
            emails.Clear();
            filteredEmails.Clear();
            ChangePage(1);
            try
            {
                statuses = (await JOLetterService.GetJOHasEmailStatus()).Where(status => status.Id is 2 or 3).ToList();
                emails = (await JOLetterService.GetApproverJobOfferHasEmail())
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
                (email.StatusId == statusId) &&
                MatchesPartial(email.JORefNum, reference) &&
                MatchesPartial(email.CandidateName, name)).ToList();
            ChangePage(1);
        }

        private void ClearSearch()
        {
            statusId = 2;
            joRefNumSearch = candidateNameSearch = string.Empty;
            SearchEmails();
        }

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

