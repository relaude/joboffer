namespace JO.Service.Services.Contracts
{
    public interface IDHService
    {
        Task<int> ApproveJobOffer(int id, int userId);
    }
}