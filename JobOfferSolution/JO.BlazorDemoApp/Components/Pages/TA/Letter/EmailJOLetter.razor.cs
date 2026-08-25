using JO.DataModel.DTOs;
using JO.DataModel.Entity;
using JO.DataModel.View;
using JO.Service.Services.Contracts;
using Microsoft.AspNetCore.Components;
using System.Globalization;
using System.Net;
using System.Text.RegularExpressions;
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
        [Inject] private IEmailService EmailService { get; set; } = default!;

        [Parameter] public int jobOfferId { get; set; }
        
        private JobOffers jobOffer = new();
        private VwJODboxCandidates vwJODboxCandidates = new();
        private VwDboxCandidates candidate = new();

        private List<JOCompanyCompensation> joCompanyCompensation = new();
        private List<JOItemLetter> joItemLetters = new();
        private List<VwCompanyCompensationItems> vwCompanyCompensationItems = new();
        
        private TextEditor? letterEditor;
        private string letterBody = string.Empty;
        private string testEmailRecipient = string.Empty;
        private string emailSubject = string.Empty;
        private bool loadLetterBody;

        private int userId;

        protected override async Task OnParametersSetAsync()
        {
            userId = await AccountService.GetJobOfferUserId();
            jobOffer = await JOLetterService.GetJobOffer(jobOfferId);
            vwJODboxCandidates = await JOLetterService.GetVwJODboxCandidates(jobOfferId);
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

        private async Task SendTestMail()
        {
            if (string.IsNullOrWhiteSpace(emailSubject))
            {
                await AlertService.Error(
                    "Enter an email subject before sending the test email.",
                    "Email Subject Required");
                return;
            }

            if (string.IsNullOrWhiteSpace(testEmailRecipient))
            {
                await AlertService.Error(
                    "Enter the email address that should receive the test email.",
                    "Test Email Recipient Required");
                return;
            }

            testEmailRecipient = testEmailRecipient.Trim();

            if (!UtilitiesService.IsValidEmail(testEmailRecipient))
            {
                await AlertService.Error(
                    "Enter a valid email address for the test email recipient.",
                    "Invalid Email Address");
                return;
            }

            if (!HasVisibleHtmlContent(letterBody))
            {
                await AlertService.Error(
                    "Enter a message in the email body before sending the test email.",
                    "Email Message Required");
                return;
            }

            bool confirmed = await AlertService.Confirm(
                $"Send a test email to {testEmailRecipient}?",
                "Send Test Email",
                "Cancel");

            if (!confirmed)
            {
                return;
            }

            EmailRequest request = new();
            request.Subject = emailSubject.Trim();
            request.To = testEmailRecipient;
            request.Body = letterBody;

            await EmailService.SendAsync(request);
            await AlertService.Success(
                $"The test email was sent to {testEmailRecipient}.",
                "Test Email Sent");
        }

        private static bool HasVisibleHtmlContent(string? html)
        {
            if (string.IsNullOrWhiteSpace(html))
            {
                return false;
            }

            string textContent = Regex.Replace(html, "<[^>]*>", string.Empty);
            textContent = WebUtility.HtmlDecode(textContent)
                .Replace('\u00A0', ' ');

            return !string.IsNullOrWhiteSpace(textContent);
        }
    }
}
