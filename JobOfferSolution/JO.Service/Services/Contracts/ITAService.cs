namespace JO.Service.Services.Contracts
{
    public interface ITAService
    {
        Task<int> AcceptJobOffer(int id, int candidateId, int userId);
        Task<int> DeclineJobOffer(int id, int reasonId, string otherReason, int userId);
        Task<int> EmailCandidate(int id, int userId);
    }
}