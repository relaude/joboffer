namespace JO.Service.Services.Contracts
{
    public interface IAlertService
    {
        Task<bool> Confirm(string title = "Are you sure?", string confirmText = "Yes", string cancelText = "Cancel");
        Task Error(string message, string title = "Error");
        Task Errors(IEnumerable<string> errors, string title = "Error");
    }
}