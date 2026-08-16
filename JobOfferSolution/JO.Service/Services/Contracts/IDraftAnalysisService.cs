using JO.DataModel.DTOs;
using JO.DataModel.View;

namespace JO.Service.Services.Contracts
{
    public interface IDraftAnalysisService
    {
        void AddProposalDto(List<OptionDto> optionDto, List<VwPckgTempHasItms> tempItems, VwDboxCandidates candidate);
        string ComputeAnnualDiffPay(List<OptionDto> optionDto, List<ComparisonDto> comparisonDto, int optionNum);
        string ComputeMonthDiffPay(List<OptionDto> optionDto, List<ComparisonDto> comparisonDto, int optionNum);
        void FillComparisonDto(List<ComparisonDto> comparisonDto, VwDboxCandidates candidate, List<OptionDto> optionDto);
        Task<List<VwPckgTempHasItms>> GetVwPckgTempHasItms(int templateId);
        void IncreasePercentage(OptionDto option, VwDboxCandidates candidate, List<ComparisonDto> comparisonDto);
        void InitProposalDto(List<OptionDto> optionDto, int numProposal, VwDboxCandidates candidate);
        void OnSelectTemplate(List<VwPckgTempHasItms> tempItems, List<ComparisonDto> comparisonDto, List<OptionDto> optionDto);
        void RemoveProposalDto(List<OptionDto> optionDto, OptionDto remove);
    }
}