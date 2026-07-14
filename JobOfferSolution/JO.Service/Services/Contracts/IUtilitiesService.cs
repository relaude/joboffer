namespace JO.Service.Services.Contracts
{
    public interface IUtilitiesService
    {
        string AttachmentRelativePath(string fullPath);
        string FormatDate(DateTime? inputDate, string format = "MMM dd, yyyy");
        string HumanizeMinutes(double? minutes);
        bool IsValidEmail(string email);
        bool IsValidSeriesOfEmail(string emails, char separator = ';');
        string LimitString(string input, int maxLength);
        string ToCurrency(decimal? amount, string? currency);
        string ToPeso(decimal? amount);
    }
}