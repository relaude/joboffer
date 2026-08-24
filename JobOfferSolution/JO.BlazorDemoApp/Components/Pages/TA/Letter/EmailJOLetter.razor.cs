using JO.DataModel.Entity;
using JO.DataModel.View;
using JO.Service.Services.Contracts;
using Microsoft.AspNetCore.Components;
using System.Globalization;
using WYSIWYGTextEditor;

namespace JO.BlazorDemoApp.Components.Pages.TA.Letter
{
    public partial class EmailJOLetter
    {
        [Inject] private IUtilitiesService UtilitiesService { get; set; } = default!;
        [Inject] private IAlertService AlertService { get; set; } = default!;
        [Inject] private IAccountService AccountService { get; set; } = default!;
        [Inject] private IJOLetterService JOLetterService { get; set; } = default!;
        [Inject] private ICompensationService CompensationService { get; set; } = default!;

        [Parameter] public int jobOfferId { get; set; }
        
        private JobOffers jobOffer = new();
        private VwDboxCandidates candidate = new();

        private List<JOCompanyCompensation> joCompanyCompensation = new();
        private List<JOItemLetter> joItemLetters = new();
        private List<VwCompanyCompensationItems> vwCompanyCompensationItems = new();
        private TextEditor? letterEditor;
        private string letterBody = string.Empty;
        private bool loadLetterBody;

        private int userId;

        protected override async Task OnParametersSetAsync()
        {
            userId = await AccountService.GetJobOfferUserId();
            jobOffer = await JOLetterService.GetJobOffer(jobOfferId);
            joCompanyCompensation = await JOLetterService.GetJOCompanyCompensation(jobOfferId);
            
            candidate = await JOLetterService.GetVwDboxCandidate(jobOffer.CandidateId.GetValueOrDefault());
            vwCompanyCompensationItems = await CompensationService.GetVwCompanyCompensationItems(jobOffer.CmpnyCmpnstnId.GetValueOrDefault());

            decimal proposedSalary = joCompanyCompensation
                .Where(jo => jo.OptionNumber > 0)
                .OrderBy(jo => jo.OptionNumber)
                .FirstOrDefault()?.ProposedSalary.GetValueOrDefault() ?? 0m;

            await LoadLetterBody(proposedSalary);
        }

        private async Task OnCompensationOptionChanged(ChangeEventArgs args)
        {
            var selectedValue = args.Value?.ToString();

            if (!decimal.TryParse(selectedValue, NumberStyles.Number, CultureInfo.InvariantCulture, out var proposedSalary)
                && !decimal.TryParse(selectedValue, NumberStyles.Number, CultureInfo.CurrentCulture, out proposedSalary))
            {
                return;
            }

            await LoadLetterBody(proposedSalary);
        }

        private async Task LoadLetterBody(decimal proposedSalary)
        {
            joItemLetters = await JOLetterService.GetJOItemLetter(jobOffer.CmpnyCmpnstnId.GetValueOrDefault());
            JOLetterService.UpdateItemLetterPlaceHolder(joItemLetters, candidate, proposedSalary);

            letterBody = string.Join(
                "<p><br></p>",
                joItemLetters
                    .Where(item => !string.IsNullOrWhiteSpace(item.MessageBody))
                    .Select(item => item.MessageBody));
            loadLetterBody = true;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (!loadLetterBody || letterEditor is null)
            {
                return;
            }

            loadLetterBody = false;

            await Task.Delay(500);
            await letterEditor.LoadHTMLContent(letterBody);
        }
    }
}
