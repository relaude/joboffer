using JO.Service.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace JO.Service.Services
{
    public class UtilitiesService : IUtilitiesService
    {
        public string ToPeso(decimal? amount)
        {
            return amount?.ToString("C", new CultureInfo("en-PH")) ?? "";
        }

        public bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            // Regular expression pattern for basic email validation
            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase);
        }

        public string LimitString(string input, int maxLength)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            if (input.Length <= maxLength)
                return input;

            return input.Substring(0, maxLength) + "...";
        }

        public string HumanizeMinutes(double? minutes)
        {
            if (minutes == null)
                return "";

            double value = minutes.Value;

            if (value < 1)
            {
                double seconds = value * 60;
                return $"{seconds:0} Second{(seconds == 1 ? "" : "s")}";
            }

            if (value < 60)
                return $"{value:0} Minute{(value == 1 ? "" : "s")}";

            double hours = value / 60;

            if (hours < 24)
                return $"{hours:0.#} Hour{(hours == 1 ? "" : "s")}";

            double days = hours / 24;

            return $"{days:0.#} Day{(days == 1 ? "" : "s")}";
        }

        public string AttachmentRelativePath(string fullPath)
        {
            if (string.IsNullOrEmpty(fullPath))
                return string.Empty;

            const string marker = "documents";
            int index = fullPath.IndexOf(marker, StringComparison.OrdinalIgnoreCase);

            if (index >= 0)
            {
                string relativePath = fullPath.Substring(index + marker.Length);

                if (!relativePath.StartsWith("\\"))
                    relativePath = "\\" + relativePath;

                relativePath = ($"{marker}{relativePath}").Replace("\\", "/");
                return relativePath;
            }

            return fullPath;
        }

        public bool IsValidSeriesOfEmail(string emails, char separator = ';')
        {
            if (string.IsNullOrWhiteSpace(emails))
                return false;

            var emailList = emails
                .Split(new[] { separator }, StringSplitOptions.RemoveEmptyEntries)
                .Select(e => e.Trim());

            foreach (var email in emailList)
            {
                if (!IsValidEmail(email))
                    return false;
            }

            return true;
        }
    }
}
