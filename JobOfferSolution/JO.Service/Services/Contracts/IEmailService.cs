using JO.DataModel.DTOs;

namespace JO.Service.Services.Contracts
{
    public interface IEmailService
    {
        Task SendAsync(EmailRequest request);
        Task<int> SendAsync(string recipients, string subject, string body);
        Task SendJOEmailNotification(int jobOfferId, int workFlowId);
        Task SendJOEmailNotification(int jobOfferId, int workFlowId, List<FileStreamDto>? FileStreams);
        Task TestMailAsync(string recipients);
    }
}
