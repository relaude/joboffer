using JO.DataModel.View;
using JO.Service.Services.Contracts;
using Microsoft.AspNetCore.Components;

namespace JO.BlazorDemoApp.Components.Pages.TA.TAEmailTemplate
{
    public partial class TemplateList
    {
        [Inject] private IEmailTemplateService EmailTemplateService { get; set; } = default!;

        private List<VwEmailTemplate> templates = [];
        private bool isLoading = true;

        protected override async Task OnInitializedAsync()
        {
            templates = await EmailTemplateService.GetVwEmailTemplate();
            isLoading = false;
        }
    }
}
