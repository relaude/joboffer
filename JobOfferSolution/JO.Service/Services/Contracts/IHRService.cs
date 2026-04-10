namespace JO.Service.Services.Contracts
{
    public interface IHRService
    {
        Task<int> ApproveJobOffer(int id, int userId);
    }
}