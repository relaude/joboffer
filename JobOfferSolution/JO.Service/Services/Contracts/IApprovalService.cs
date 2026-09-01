using JO.DataModel.DTOs;
using JO.DataModel.Entity;

namespace JO.Service.Services.Contracts
{
    public interface IApprovalService
    {
        Task<int> DHApprovals(List<ProposalDto> joProposal);
        Task<List<ProposalDto>> GetProposalDto(int jobOfferId);
        Task<int> HRApprovals(List<ProposalDto> joProposal);
        Task JobOfferActionFlowStatus(int jobOfferId, int workFlowId, int roleId, int actionId, int userId);
        Task JobOfferActionFlowStatus(int jobOfferId, int workFlowId, int roleId, int actionId, int userId, string remarks);
        Task JobOfferActionSendBack(int jobOfferId, int roleId, int userId, string remarks);
        Task JobOfferChangeStatus(int jobOfferId, int workFlowId);
        Task JobOfferChangeStatus(int jobOfferId, int statusId, int workFlowId);
        Task<int> PresApprovals(List<ProposalDto> joProposal);
    }
}