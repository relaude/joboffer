using JO.DataModel.Entity;
using JO.DataModel.View;

namespace JO.Service.Services.Contracts
{
    public interface ITransactionService
    {
        Task<IEnumerable<VwJobOfferTransactions>> GetAllTransaction();
        Task<IEnumerable<JobOfferPackages>> GetPackages(int id);
        Task<IEnumerable<VwTransactionAttachments>> GetSubmittedFiles(int id);
        Task<VwJobOfferTransactions> GetTransactionDetails(int id);
        Task<int> SetTransactionStatus(int id, int statusId);
    }
}