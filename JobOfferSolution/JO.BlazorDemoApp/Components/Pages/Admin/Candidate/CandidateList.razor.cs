using JO.DataModel.DTOs;
using JO.DataModel.View;
using JO.Service.Extensions;
using JO.Service.Services.Contracts;
using Microsoft.AspNetCore.Components;


namespace JO.BlazorDemoApp.Components.Pages.Admin.Candidate
{
    public partial class CandidateList
    {
        [Inject] private IDBoxCandidateService DBoxCandidateService { get; set; } = default!;

        private List<VwDboxCandidates> responses = new();
        private List<VwDboxCandidates> filteredResponses = new();
        private PagedResult<VwDboxCandidates> pagedResponses = new() { Page = 1, PageSize = 10 };
        private string candidateDBoxIdSearch = string.Empty;
        private string candidateNameSearch = string.Empty;
        private string jobPositionSearch = string.Empty;
        private string companySearch = string.Empty;
        private string divisionSearch = string.Empty;
        private string departmentSearch = string.Empty;
        private bool isLoading = true;
        private string? loadError;

        protected override async Task OnInitializedAsync() => await LoadResponsesAsync();

        private async Task LoadResponsesAsync()
        {
            isLoading = true;
            loadError = null;
            responses.Clear();
            filteredResponses.Clear();
            ChangePage(1);
            try
            {
                responses = await DBoxCandidateService.GetVwDboxCandidates();
                SearchResponses();
            }
            catch (Exception)
            {
                loadError = "Unable to load candidates. Please try again.";
            }
            finally
            {
                isLoading = false;
            }
        }

        private void ChangePage(int page) =>
            pagedResponses = filteredResponses.ToPagedResult(page, pagedResponses.PageSize);

        private void ChangePageSize(int pageSize) =>
            pagedResponses = filteredResponses.ToPagedResult(1, pageSize);

        private void SearchResponses()
        {
            var id = candidateDBoxIdSearch.Trim();
            var name = candidateNameSearch.Trim();
            var jobPosition = jobPositionSearch.Trim();
            var company = companySearch.Trim();
            var division = divisionSearch.Trim();
            var department = departmentSearch.Trim();
            filteredResponses = responses.Where(candidate =>
                MatchesPartial(candidate.DboxRefNum, id) &&
                MatchesPartial(candidate.CandidateName, name) &&
                MatchesPartial(candidate.JobPosition, jobPosition) &&
                MatchesPartial(candidate.Company, company) &&
                MatchesPartial(candidate.Division, division) &&
                MatchesPartial(candidate.Department, department)).ToList();
            ChangePage(1);
        }

        private void ClearSearch()
        {
            candidateDBoxIdSearch = candidateNameSearch = string.Empty;
            jobPositionSearch = companySearch = divisionSearch = departmentSearch = string.Empty;
            SearchResponses();
        }

        // In-memory equivalent of a case-insensitive SQL LIKE '%term%' search.
        private static bool MatchesPartial(string? value, string term) =>
            term.Length == 0 || (value?.Contains(term, StringComparison.OrdinalIgnoreCase) ?? false);

        private static string DisplayValue(string? value) => string.IsNullOrWhiteSpace(value) ? "-" : value;

    }
}

