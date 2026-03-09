using JO.DataModel.DTOs;

namespace JO.Service.Services.Contracts
{
    public interface IEmailService
    {
        Task SendAsync(EmailRequest request);
        Task TestMailAsync(string recipients);
    }
}