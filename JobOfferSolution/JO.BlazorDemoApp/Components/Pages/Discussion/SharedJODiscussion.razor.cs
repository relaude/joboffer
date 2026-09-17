using JO.DataModel.DTOs;
using JO.DataModel.Entity;
using JO.DataModel.View;
using JO.Service.Services.Contracts;
using Microsoft.AspNetCore.Components;

namespace JO.BlazorDemoApp.Components.Pages.Discussion
{
    public partial class SharedJODiscussion
    {
        [Inject] private IAlertService AlertService { get; set; } = default!;
        [Inject] private IUtilitiesService UtilitiesService { get; set; } = default!;

        [Parameter, EditorRequired] public DiscussionDto Discussion { get; set; } = new();
        //[Parameter, EditorRequired] public IReadOnlyList<ChannelTypes> Channels { get; set; } = [];
        //[Parameter, EditorRequired] public IReadOnlyList<DiscussSteps> Steps { get; set; } = [];
        [Parameter, EditorRequired] public IReadOnlyList<DiscussionStatus> Status { get; set; } = [];
        [Parameter, EditorRequired] public IReadOnlyList<JODeclineReason> DeclineReason { get; set; } = [];
        //[Parameter, EditorRequired] public IReadOnlyList<CandResponse> Responses { get; set; } = [];
        [Parameter, EditorRequired] public IReadOnlyList<JOCompanyCompensation> Proposals { get; set; } = [];
        [Parameter, EditorRequired] public IReadOnlyList<VwDiscussions> Discussions { get; set; } = [];
        [Parameter, EditorRequired] public EventCallback<DiscussionDto> OnSave { get; set; }
        [Parameter] public bool RequireDiscussionNotes { get; set; } = true;
        [Parameter] public bool IsSaving { get; set; }
        [Parameter] public string ComponentId { get; set; } = "shared-jo-discussion";

        private bool IsSaveDisabled => IsSaving || Discussions.Any(discussion => discussion.StatusId == 4);

        private DateTime? DiscussionDate { get; set; }
        private string? DiscussionHour { get; set; }
        private string? DiscussionMinute { get; set; }
        private string? DiscussionPeriod { get; set; }
        private DiscussionDto? boundDiscussion;
        private DateTime? boundDiscussAt;

        protected override void OnParametersSet()
        {
            // Keep edits during parent renders, but refresh when the model is replaced or reset.
            if (!ReferenceEquals(boundDiscussion, Discussion) || boundDiscussAt != Discussion.DiscussAt)
            {
                boundDiscussion = Discussion;
                boundDiscussAt = Discussion.DiscussAt;
                DiscussionDate = Discussion.DiscussAt?.Date;
                var time = Discussion.DiscussAt;
                DiscussionHour = time.HasValue ? (time.Value.Hour % 12 == 0 ? 12 : time.Value.Hour % 12).ToString() : null;
                DiscussionMinute = time?.Minute.ToString("00");
                DiscussionPeriod = time.HasValue ? (time.Value.Hour < 12 ? "AM" : "PM") : null;
            }
        }

        private void UpdateDiscussionDate(ChangeEventArgs args)
        {
            DiscussionDate = DateTime.TryParseExact(args.Value?.ToString(), "yyyy-MM-dd",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None, out var date)
                ? date
                : null;
        }

        private string DiscussionDateId => $"{ComponentId}-date";
        private string DiscussionHourId => $"{ComponentId}-hour";
        private string DiscussionMinuteId => $"{ComponentId}-minute";
        private string DiscussionPeriodId => $"{ComponentId}-period";
        private string ChannelId => $"{ComponentId}-channel";
        private string StepId => $"{ComponentId}-step";
        private string StatusId => $"{ComponentId}-status";
        private string ResponseId => $"{ComponentId}-response";
        private string ProposalId => $"{ComponentId}-proposal";
        private string NotesId => $"{ComponentId}-notes";
        private string FeedbackId => $"{ComponentId}-feedback";

        private async Task SaveAsync()
        {
            if (IsSaveDisabled)
                return;

            var errors = new List<string>();

            if (!Discussion.StatusId.HasValue
                || !Status.Any(status => status.Id == Discussion.StatusId.Value))
                errors.Add("Select a valid status before saving the discussion.");

            if (!Proposals.Any(proposal => proposal.Id == Discussion.ProposalId
                && proposal.OptionNumber > 0 && proposal.Declined != true))
                errors.Add("Select an available proposal before saving the discussion.");

            if (!DiscussionDate.HasValue)
                errors.Add("Enter a valid discussion date.");

            //if (RequireDiscussionNotes && string.IsNullOrWhiteSpace(Discussion.Comments))
            //    errors.Add("Discussion Notes are required.");

            //if (string.IsNullOrWhiteSpace(Discussion.FeedBack))
            //    errors.Add("Division Head remarks is required.");

            if (Discussion.StatusId == 4 && !Discussion.DeclineReasonId.HasValue)
                errors.Add("Decline reason is required for a declined offer.");

            if (Discussion.StatusId == 4 && Discussion.DeclineReasonId == 5
                && string.IsNullOrWhiteSpace(Discussion.DeclineRemarks))
                errors.Add("Decline remarks are required when the decline reason is Others.");

            if (errors.Any())
            {
                await AlertService.Errors(errors, "Required Fields");
                return;
            }

            // The active date-only input must be copied to the DTO before saving.
            Discussion.DiscussAt = DiscussionDate!.Value.Date;
            boundDiscussAt = Discussion.DiscussAt;

            await OnSave.InvokeAsync(Discussion);
        }
    }
}
