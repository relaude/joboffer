using JO.DataModel.Entity;
using JO.DataModel.View;

namespace JO.Service.Services.Contracts
{
    public interface IEmailTemplateService
    {
        Task UpdateEmailTemplate(EmailTemplate emailTemplate, HashSet<int> selectedRecipientRoleIds);
        Task<int> CreateEmailTemplate(EmailTemplate emailTemplate, HashSet<int> selectedRecipientRoleIds);
        Task<EmailTemplate> GetEmailTemplate(int templateId);
        Task<List<EmailTemplateRoles>> GetEmailTemplateRoles();
        Task<List<EmailTemplateRoles>> GetEmailTemplateRoles(int templateId);
        Task<List<JOWorkFlowStatus>> GetJOWorkFlowStatus();
        Task<List<VwJOUserRoles>> GetVwJOUserRoles();
        Task<List<VwEmailTemplate>> GetVwEmailTemplate();
        Task<List<JOWorkFlowStatus>> GetFilteredJOWorkFlowStatus();
    }
}
