using JO.DataModel.Entity;
using JO.DataModel.View;

namespace JO.Service.Services.Contracts
{
    public interface IJOAnalysisService
    {
        Task<List<VwCompensationBenefits>> GetCompensationBenefits(int packageId);
        Task<List<SalaryBandStatus>> GetValidationStatus();
        List<Proposal> InitializeProposal(int jobOfferId, int salaryBandId, decimal current, VwSalaryMatrixBand matrixBand, List<VwCompensationBenefits> compBen);
        Task<int> LegalEntitySetup(LegalEntities legal);
        List<Proposal> ReComputeAnalysis(List<Proposal> proposals, VwSalaryMatrixBand matrixBand, List<VwCompensationBenefits> compBen);
        Task<int> SaveProposal(List<Proposal> proposals);
    }
}