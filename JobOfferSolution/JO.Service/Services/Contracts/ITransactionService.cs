using JO.DataModel.View;

namespace JO.Service.Services.Contracts
{
    public interface ITransactionService
    {
        Task<IEnumerable<VwTransactionAttachments>> GetSubmittedFiles(int id);
        Task<VwJobOfferTransactions> GetTransactionDetails(int id);
    }
}