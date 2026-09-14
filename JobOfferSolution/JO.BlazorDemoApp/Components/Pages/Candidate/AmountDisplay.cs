namespace JO.BlazorDemoApp.Components.Pages.Candidate
{
    internal static class AmountDisplay
    {
        public static string Format(decimal? value) => value?.ToString("N2") ?? "-";

        public static string Format(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return "-";

            return decimal.TryParse(value, out var amount) ? amount.ToString("N2") : value;
        }
    }
}
