using JO.DataModel.DTOs;
using JO.DataModel.Entity;
using JO.DataModel.View;
using JO.Service.Services.Contracts;
using Microsoft.AspNetCore.Components;

namespace JO.BlazorDemoApp.Components.Pages.TA.Discussion
{
    public partial class SharedJODiscussion
    {
        [Inject] private IUtilitiesService UtilitiesService { get; set; } = default!;

        [Parameter, EditorRequired] public DiscussionDto Discussion { get; set; } = new();
        [Parameter, EditorRequired] public IReadOnlyList<ChannelTypes> Channels { get; set; } = [];
        [Parameter, EditorRequired] public IReadOnlyList<DiscussSteps> Steps { get; set; } = [];
        [Parameter, EditorRequired] public IReadOnlyList<CandResponse> Responses { get; set; } = [];
        [Parameter, EditorRequired] public IReadOnlyList<JOCompanyCompensation> Proposals { get; set; } = [];
        [Parameter, EditorRequired] public IReadOnlyList<VwDiscussions> Discussions { get; set; } = [];
        [Parameter, EditorRequired] public EventCallback<DiscussionDto> OnSave { get; set; }
        [Parameter] public bool IsSaving { get; set; }
        [Parameter] public string ComponentId { get; set; } = "shared-jo-discussion";

        private string DiscussionDateId => $"{ComponentId}-date";
        private string ChannelId => $"{ComponentId}-channel";
        private string StepId => $"{ComponentId}-step";
        private string ResponseId => $"{ComponentId}-response";
        private string ProposalId => $"{ComponentId}-proposal";
        private string NotesId => $"{ComponentId}-notes";
        private string FeedbackId => $"{ComponentId}-feedback";

        private Task SaveAsync()
        {
            return OnSave.InvokeAsync(Discussion);
        }
    }
}
