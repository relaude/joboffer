using JO.DataModel.DTOs;

namespace JO.Service.Services.Contracts
{
    public interface IEmailService
    {
        Task SendAsync(EmailRequest request);
        Task<int> SendAsync(string recipients, string subject, string body);
        Task TestMailAsync(string recipients);
    }
}