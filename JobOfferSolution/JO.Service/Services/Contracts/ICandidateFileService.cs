using JO.DataModel.Entity;

namespace JO.Service.Services.Contracts
{
    public interface ICandidateFileService
    {
        Task<List<DocumentType>> GetDocumentType();
        Task<DboxCandidates?> GetCandidateById(int candidateId);
        Task<List<CandidateFiles>> GetCandidateFiles(int candidateId);
        Task<int> InsertCandidateFiles(CandidateFiles file, Stream content);
        Task<(string Path, string FileName)?> GetDownload(int candidateId, int fileId);
    }
}
