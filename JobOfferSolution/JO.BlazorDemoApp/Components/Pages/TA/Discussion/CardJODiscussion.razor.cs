using JO.DataModel.View;
using JO.Service.Services.Contracts;
using Microsoft.AspNetCore.Components;

namespace JO.BlazorDemoApp.Components.Pages.TA.Discussion
{
    public partial class CardJODiscussion
    {
        [Inject] private IUtilitiesService UtilitiesService { get; set; } = default!;

        [Parameter, EditorRequired]
        public IReadOnlyList<VwDiscussions> Discussions { get; set; } = [];

        [Parameter]
        public string Title { get; set; } = "Discussion Timeline";

        [Parameter]
        public string Subtitle { get; set; } = "Recent contact history and candidate signals.";

        [Parameter]
        public string EmptyMessage { get; set; } = "No discussion history is available.";
    }
}
