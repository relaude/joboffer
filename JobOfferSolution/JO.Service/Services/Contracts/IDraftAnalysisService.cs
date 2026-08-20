using JO.DataModel.DTOs;
using JO.DataModel.Entity;
using JO.DataModel.View;

namespace JO.Service.Services.Contracts
{
    public interface IDraftAnalysisService
    {
        void AddProposalDto(List<OptionDto> optionDto, List<VwPckgTempHasItms> tempItems, VwDboxCandidates candidate);
        string ComputeAnnualDiffPay(List<OptionDto> optionDto, List<ComparisonDto> comparisonDto, int optionNum);
        string ComputeMonthDiffPay(List<OptionDto> optionDto, List<ComparisonDto> comparisonDto, int optionNum);
        void FillComparisonDto(List<ComparisonDto> comparisonDto, VwDboxCandidates candidate, List<OptionDto> optionDto);
        Task<List<CompensationDto>> GetCompensationDto();
        Task<List<CompensationOptions>> GetCompensationOptions(List<int> packageIds);
        Task<List<CompensationOptions>> GetCompensationOptions(int jobOfferId);
        Task<List<CompensationPackage>> GetCompensationPackage();
        Task<List<CompensationPackage>> GetCompensationPackage(int jobOfferId);
        Task<List<CompensationTemplate>> GetCompensationTemplate();
        Task<List<CompensationTemplateItems>> GetCompensationTemplateItems();
        Task<List<VwCompensationTemplateItems>> GetVwCompensationTemplateItems();
        Task<List<VwPckgTempHasItms>> GetVwPckgTempHasItms(int templateId);
        void IncreasePercentage(OptionDto option, VwDboxCandidates candidate, List<ComparisonDto> comparisonDto);
        void InitProposalDto(List<OptionDto> optionDto, int numProposal, VwDboxCandidates candidate);
        void OnSelectTemplate(List<VwPckgTempHasItms> tempItems, List<ComparisonDto> comparisonDto, List<OptionDto> optionDto);
        void RemoveProposalDto(List<OptionDto> optionDto, OptionDto remove);
        Task<int> SaveAnalysis(List<CompensationPackage> compenPackageOptions, List<CompensationOptions> compenOptions, int templateId, int userId);
        Task<List<CompenItemCategoryDto>> SetUpCompenItemCategoryDto();
    }
}