namespace JO.Service.Services.Contracts
{
    public interface IJobOfferDocumentService
    {
        /// <summary>
        /// Creates or refreshes the latest draft using template 1 and saves candidate-named benefits and option PDFs
        /// in JobOfferDocuments. Existing documents are preserved and missing documents are added. No email is sent.
        /// </summary>
        Task SaveJobOfferEmailAsync(int jobOfferId);
    }
}
