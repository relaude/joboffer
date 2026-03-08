namespace JO.Service.Services.Contracts
{
    public interface IEmailService
    {
        Task TestMailAsync(string recipients);
    }
}