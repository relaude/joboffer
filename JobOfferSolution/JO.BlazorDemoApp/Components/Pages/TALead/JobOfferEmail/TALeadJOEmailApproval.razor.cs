using JO.DataModel.View;
using JO.Service.Services.Contracts;
using Microsoft.AspNetCore.Components;

namespace JO.BlazorDemoApp.Components.Pages.TALead.JobOfferEmail
{
    public partial class TALeadJOEmailApproval
    {
        [Inject] private IDiscussionService DiscussionService { get; set; } = default!;

        [Parameter] public int JobOfferId { get; set; }
        [Parameter] public int EmailId { get; set; }

        private TabName activeTab = TabName.EmailApproval;
        private List<VwDiscussions> vwDiscussions = [];

        protected override async Task OnParametersSetAsync()
        {
            vwDiscussions = await DiscussionService.GetDiscussions(JobOfferId);
        }

        private void SelectTab(TabName tab)
        {
            activeTab = tab;
        }

        private enum TabName
        {
            EmailApproval,
            ActionLogs,
            Discussion
        }
    }
}
