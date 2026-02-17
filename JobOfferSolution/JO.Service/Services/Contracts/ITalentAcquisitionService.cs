namespace JO.Service.Services.Contracts
{
    public interface ITalentAcquisitionService
    {
        Task<int> CreatePackage(int transactionId, string packageName, decimal packageAmount, int priority);
        Task<int> UpdatePackage(int id, string packageName, decimal packageAmount, int priority);
    }
}