using JO.DataModel.DTOs;
using JO.DataModel.Entity;
using JO.DataModel.View;
using JO.Service.Constants;
using JO.Service.Services.Contracts;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace JO.BlazorDemoApp.Components.Pages.TA.Candidate
{
    public partial class CandidateResponses
    {
        [Inject] private ICandidateService CandidateService { get; set; } = default!;

    private List<DataModel.Entity.CandidateResponses> candidateResponses = new();

    protected override async Task OnInitializedAsync()
    {
        candidateResponses = await CandidateService.GetCandidateResponses();
    }
    }
}
