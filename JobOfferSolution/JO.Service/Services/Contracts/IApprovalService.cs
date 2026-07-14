using JO.DataModel.Entity;

namespace JO.Service.Services.Contracts
{
    public interface IApprovalService
    {
        Task<int> SaveDivisionHeadApprovals(List<DHJOProposal> dhProposal);
    }
}