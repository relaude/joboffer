namespace JO.Service.Services.Contracts
{
    public interface IUtilitiesService
    {
        string AttachmentRelativePath(string fullPath);
        string HumanizeMinutes(double? minutes);
        bool IsValidEmail(string email);
        bool IsValidSeriesOfEmail(string emails, char separator = ';');
        string LimitString(string input, int maxLength);
        string ToPeso(decimal? amount);
    }
}