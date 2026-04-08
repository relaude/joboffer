namespace JO.Service.Services.Contracts
{
    public interface IReturnJobOfferService
    {
        Task<int> ReturnToTA(int id, int reasonId, int userId, string remarks);
    }
}