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
        private List<VwJOApprovalFlow> approvalFlow = [];
        private bool isLoading;

        protected override async Task OnParametersSetAsync()
        {
            isLoading = true;
            try
            {
                actionLogs = await JOLogsService.GetVwJOActionLogs(JobOfferId);
                approvalFlow = await JOLogsService.GetVwJOApprovalFlow(JobOfferId);
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
            9 => "fa-times-circle",
            10 => "fa-check-circle",
            11 => "fa-envelope",
            12 => "fa-check-circle",
            13 => "fa-undo-alt",
            14 => "fa-paper-plane",
            _ => "fa-history"
        };

        private static string GetMarkerClass(int? actionId) => actionId switch
        {
            1 or 2 or 11 => "is-blue",
            3 => "is-purple",
            4 or 6 or 7 or 10 or 12 or 14 => "is-green",
            5 or 9 or 13 => "is-red",
            8 => "is-amber",
            _ => "is-gray"
        };

        private static string GetBadgeClass(int? actionId) => actionId switch
        {
            4 or 6 or 7 or 14 => "badge-success",
            5 or 9 => "badge-danger",
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
            8 => "For Negotiation",
            9 => "Declined",
            10 => "2nd Approved",
            11 => "Email for Approval ",
            12 => "Email Approved",
            13 => "Email Send Back",
            14 => "Email Sent",
            _ => "Activity"
        };

        private static string GetApprovalFlowStatus(VwJOApprovalFlow step) =>
            step.IsAproved == true
                ? step.RoleId switch
                {
                    1 => "Prepared",
                    2 => "Reviewed",
                    _ => "Approved"
                }
                : "Pending";

        private static string DisplayValue(string? value, string fallback = "-") =>
            string.IsNullOrWhiteSpace(value) ? fallback : value;
    }
}
