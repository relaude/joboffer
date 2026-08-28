using JO.DataModel.View;
using JO.Service.Services.Contracts;
using Microsoft.AspNetCore.Components;

namespace JO.BlazorDemoApp.Components.Pages.JobOffer
{
    public partial class CardJOActionLogs
    {
        [Inject] private IJOLogsService JOLogsService { get; set; } = default!;

        [Parameter, EditorRequired] public int JobOfferId { get; set; }
        [Parameter] public string Title { get; set; } = "Action History";
        [Parameter] public string Subtitle { get; set; } = "Job offer activity, decisions, and workflow changes.";
        [Parameter] public string EmptyMessage { get; set; } = "No action history is available for this job offer.";

        private List<VwJOActionLogs> actionLogs = [];
        private bool isLoading;

        protected override async Task OnParametersSetAsync()
        {
            isLoading = true;
            try
            {
                actionLogs = await JOLogsService.GetVwJOActionLogs(JobOfferId);
            }
            finally
            {
                isLoading = false;
            }
        }

        private static string GetActionIcon(int? actionId) => actionId switch
        {
            1 => "fa-plus-circle",
            2 => "fa-file-signature",
            3 => "fa-search",
            4 => "fa-check-circle",
            5 => "fa-undo-alt",
            6 => "fa-handshake",
            7 => "fa-flag-checkered",
            8 => "fa-comments-dollar",
            _ => "fa-history"
        };

        private static string GetMarkerClass(int? actionId) => actionId switch
        {
            1 or 2 => "is-blue",
            3 => "is-purple",
            4 or 6 or 7 => "is-green",
            5 => "is-red",
            8 => "is-amber",
            _ => "is-gray"
        };

        private static string GetBadgeClass(int? actionId) => actionId switch
        {
            4 or 6 or 7 => "badge-success",
            5 => "badge-danger",
            8 => "badge-warning",
            3 => "badge-info",
            _ => "badge-primary"
        };

        private static string GetActionCategory(int? actionId) => actionId switch
        {
            1 => "Created",
            2 => "Prepared",
            3 => "Reviewed",
            4 => "Approved",
            5 => "Returned",
            6 => "Accepted",
            7 => "Completed",
            8 => "Negotiation",
            _ => "Activity"
        };

        private static string DisplayValue(string? value, string fallback) =>
            string.IsNullOrWhiteSpace(value) ? fallback : value;
    }
}
