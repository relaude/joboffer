using JO.DataModel.DTOs;
using JO.DataModel.Entity;

namespace JO.Service.Services.Contracts
{
    public interface IApprovalService
    {
        Task<int> DHApprovals(List<ProposalDto> joProposal);
        Task<List<ProposalDto>> GetProposalDto(int jobOfferId);
    }
}