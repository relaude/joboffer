using CurrieTechnologies.Razor.SweetAlert2;
using JO.Service.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace JO.Service.Services
{
    public class AlertService : IAlertService
    {
        private readonly SweetAlertService _swal;

        public AlertService(SweetAlertService swal)
        {
            _swal = swal;
        }

        public async Task Success(string message = "Successfully saved.", string title = "Success") => await _swal.FireAsync(title, message, SweetAlertIcon.Success);

        public async Task Error(string message, string title = "Error")
        {
            await _swal.FireAsync(title, message, SweetAlertIcon.Error);
        }

        public async Task Errors(IEnumerable<string> errors, string title = "Error")
        {
            if (errors == null || !errors.Any())
                return;

            var html = BuildUnorderedList(errors);

            await _swal.FireAsync(new SweetAlertOptions
            {
                Icon = SweetAlertIcon.Error,
                Title = title,
                Html = html
            });
        }

        public async Task<bool> Confirm(string title = "Are you sure?",
            string confirmText = "Yes",
            string cancelText = "Cancel")
        {
            var confirmResult = await _swal.FireAsync(new SweetAlertOptions
            {
                Icon = SweetAlertIcon.Question,
                Title = title,
                ShowCancelButton = true,
                ConfirmButtonText = confirmText,
                CancelButtonText = cancelText
            });

            return confirmResult.IsConfirmed;
        }

        public async Task<int> ConfirmProposalNumber(string inputValue = "4")
        {
            var confirmResult = await _swal.FireAsync(new SweetAlertOptions
            {
                Icon = SweetAlertIcon.Question,
                Title = "How many proposal?",
                Input = SweetAlertInputType.Number,
                InputValue = inputValue,
                InputAttributes = new Dictionary<string, string>
                {
                    ["min"] = "1",
                    ["max"] = "4"
                },
                ShowCancelButton = true,
                ConfirmButtonText = "Continue",
                CancelButtonText = "Cancel"
            });

            return confirmResult.IsConfirmed &&
                   int.TryParse(confirmResult.Value, out var proposalNumber) &&
                   proposalNumber >= 1 &&
                   proposalNumber <= 4
                ? proposalNumber
                : 0;
        }

        private string BuildUnorderedList(IEnumerable<string> items)
        {
            var sb = new StringBuilder();

            sb.Append("<ul style='text-align:left; padding-left:20px; margin-top:10px;'>");

            foreach (var item in items)
            {
                sb.Append($"<li>{item}</li>");
            }

            sb.Append("</ul>");

            return sb.ToString();
        }
    }
}
