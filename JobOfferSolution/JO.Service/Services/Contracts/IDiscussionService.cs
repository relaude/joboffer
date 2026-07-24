using JO.DataModel.DTOs;
using JO.DataModel.Entity;
using JO.DataModel.View;

namespace JO.Service.Services.Contracts
{
    public interface IDiscussionService
    {
        Task<List<Proposal>> GetApprovedProposal(int jobOfferId);
        Task<List<CandResponse>> GetCandResponse();
        Task<List<ChannelTypes>> GetChannelTypes();
        Task<List<VwDiscussions>> GetDiscussions(int jobOfferId);
        Task<List<DiscussSteps>> GetDiscussSteps();
        Task<int> SaveDiscussion(Discussions discussion);
        Task<int> SaveDiscussion(DiscussionDto dto);
    }
}