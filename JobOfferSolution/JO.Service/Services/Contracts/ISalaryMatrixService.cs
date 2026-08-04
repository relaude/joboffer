using JO.DataModel.Entity;
using JO.DataModel.View;

namespace JO.Service.Services.Contracts
{
    public interface ISalaryMatrixService
    {
        Task<int> CreateMatrix(SalaryMatrix matrix, List<SalaryMatrixBand> salaryBands);
        Task<VwSalaryMatrix> GetMatrix(int matrixId);
        Task<List<VwSalaryMatrix>> GetMatrixList();
        Task<List<VwSalaryMatrixBand>> GetSalaryBands(int matrixId);
        Task<List<VwSalaryMatrixBand>> GetSalaryBandsByJOId(int jobOfferId);
        Task<int> UpdateMatrixEffectiveDate(int matrixId, DateTime effectiveTo, bool isActive, int modifiedBy);

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
    }
}
