using JO.DataModel.DTOs;
using JO.DataModel.Entity;
using JO.DataModel.View;
using JO.Service.Constants;
using JO.Service.Services.Contracts;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace JO.BlazorDemoApp.Components.Pages.Admin.Candidate
{
    public partial class DboxCandidateList
    {
        [Inject] private ICandidateService CandidateService { get; set; } = default!;

    private List<DboxCandidates> candidates = new();

    protected override async Task OnInitializedAsync()
    {
        candidates = await CandidateService.GetDboxCandidates();
    }
    }
}
