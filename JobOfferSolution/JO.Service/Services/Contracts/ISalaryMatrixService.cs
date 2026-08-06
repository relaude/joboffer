using JO.DataModel.DTOs;
using JO.DataModel.Entity;
using JO.DataModel.View;

namespace JO.Service.Services.Contracts
{
    public interface ISalaryMatrixService
    {
        Task<VwSalaryMatrix> GetVwSalaryMatrix(int matrixId);
        Task<List<VwSalaryMatrixBand>> GetSalaryBands(int matrixId);
        Task<List<VwSalaryMatrixBand>> GetSalaryBandsByJOId(int jobOfferId);
        Task<int> SaveJobFamily(JobFamilies jobFamily);
        Task<int> UpdateJobFamily(JobFamilies jobFamily);
        Task<List<JobFamilies>> GetJobFamilies();

        Task<int> SaveJobLevel(JobLevels jobLevel);
        Task<int> UpdateJobLevel(JobLevels jobLevel);
        Task<List<JobLevels>> GetJobLevels();

        Task<int> SaveJobPosition(JobPositions jobPosition);
        Task<int> UpdateJobPosition(JobPositions jobPosition);
        Task<List<JobPositions>> GetJobPositions();
        Task<List<VwCompanySalaryGrades>> GetCompanySalaryGrades(int companyId);
        Task<int> CreateMatrix(SalaryMatrix matrix, List<SalaryBandsDto> salaryBands);
        Task<List<VwSalaryBands>> GetVwSalaryBands(int matrixId);
        Task<List<VwSalaryMatrix>> GetVwSalaryMatrix();
        Task<SalaryMatrix> GetSalaryMatrix(int matrixId);
        Task<int> UpdateMatrix(SalaryMatrix matrix, List<SalaryBandsDto> salaryBands);
        Task<List<DropdownDto>> GetNewMatrixCompanies();
    }
}
