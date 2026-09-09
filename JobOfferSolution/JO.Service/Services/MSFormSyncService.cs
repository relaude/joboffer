using JO.DataModel.Entity;
using JO.Persistence.DataAccess;
using JO.Service.Services.Contracts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace JO.Service.Services
{
    public class MSFormSyncService : IMSFormSyncService
    {
        private readonly IWebHostEnvironment _env;
        private readonly IDbContextFactory<JobOfferDbContext> _dbContext;
        private string ExcelDestinationPath => Path.Combine(_env.WebRootPath, "excel");

        public MSFormSyncService(IWebHostEnvironment env,
            IDbContextFactory<JobOfferDbContext> dbContext)
        {
            _env = env;
            _dbContext = dbContext;
        }

        public async Task<DateTime?> GetLatestDateTimeMSFormAsync()
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            return await context.MSFormSyncLogs.MaxAsync(jo => jo.SyncDate);
        }

        public async Task<List<CandidateResponseRawData>> SaveCandidateResponseRawData(int createdBy)
        {
            await using var context = await _dbContext.CreateDbContextAsync();

            var responseRawData = await GetCandidateResponses(createdBy);
            ResponseValidation(responseRawData);
            await context.CandidateResponseRawData.AddRangeAsync(responseRawData);
            await context.SaveChangesAsync();

            MSFormSyncLogs syncLog = new MSFormSyncLogs
            {
                SyncDate = DateTime.Now,
                TotalRows = responseRawData.Count,
                InvalidCount = responseRawData.Where(jo=>jo.InvalidResponse == true).Count(),
            };
            await context.MSFormSyncLogs.AddAsync(syncLog);
            await context.SaveChangesAsync();

            return responseRawData;
        }

        public async Task<List<CandidateResponseRawData>> GetCandidateResponses(int createdBy)
        {
            List<CandidateResponseRawData> rawData = new();

            var filePath = Path.Combine(ExcelDestinationPath, "CandidateResponseSample.xlsx");
            if (!File.Exists(filePath))
                return rawData;

            // Use the same EPPlus license configuration as MassUploadService.
            ExcelPackage.License.SetNonCommercialOrganization("JobOffer");
            using var package = new ExcelPackage();
            await package.LoadAsync(new FileInfo(filePath));

            var worksheet = package.Workbook.Worksheets.FirstOrDefault();
            if (worksheet?.Dimension is null)
                return rawData;

            var columns = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            var headerRow = worksheet.Dimension.Start.Row;
            for (var column = worksheet.Dimension.Start.Column; column <= worksheet.Dimension.End.Column; column++)
            {
                var header = NormalizeHeader(worksheet.Cells[headerRow, column].Text);
                if (header.Length > 0)
                    columns.TryAdd(header, column);
            }

            if (!columns.ContainsKey(NormalizeHeader("Candidate DBoxID"))
                && !columns.ContainsKey(NormalizeHeader("CandidateDBoxID")))
            {
                throw new ValidationException(
                    "CandidateResponseSample.xlsx is missing the required 'Candidate DBoxID' column. " +
                    "Update the source workbook (header in B1), populate the candidate IDs, and download it again before syncing.");
            }

            var createdAt = DateTime.Now;
            for (var row = headerRow + 1; row <= worksheet.Dimension.End.Row; row++)
            {
                if (!columns.Values.Any(column => !string.IsNullOrWhiteSpace(worksheet.Cells[row, column].Text)))
                    continue;

                string? Read(params string[] headers)
                {
                    foreach (var header in headers)
                    {
                        if (columns.TryGetValue(NormalizeHeader(header), out var column))
                        {
                            var value = worksheet.Cells[row, column].Text.Trim();
                            return string.IsNullOrWhiteSpace(value) ? null : value;
                        }
                    }
                    return null;
                }

                rawData.Add(new CandidateResponseRawData
                {
                    CandidateResponseId = Read("Id"),
                    CandidateDBoxID = Read("Candidate DBoxID", "CandidateDBoxID"),
                    ResponseStartedAt = Read("Start time"),
                    ResponseCompletedAt = Read("Completion time"),
                    EmailAddress = Read("Email"),
                    RespondentName = Read("Name"),
                    HasDataPrivacyConsent = Read("Consent statement", "By proceeding with this form, you consent to the collection, generation, use, processing, storage and retention of your personal data by the Company for your application process."),
                    CandidateFullName = Read("Full Name (First Name, Last Name)"),
                    FormCompletedDate = Read("Date Form is Accomplished"),
                    PositionAppliedFor = Read("Position Applied For"),
                    UnilabDivision = Read("Unilab Division"),
                    ExpectedMonthlyBasicSalary = Read("Expected Monthly Basic Salary"),
                    Age = Read("age"),
                    EmploymentStatus = Read("Employment Status"),
                    RelevantExperience = Read("Experience"),
                    CurrentEmployerName = Read("Name of Current Employer"),
                    LastEmployerIndustry = Read("Industry of Last Employer"),
                    LastPositionHeld = Read("Last Position Held"),
                    CurrentMonthlyBasicSalary = Read("Monthly Basic Salary"),
                    GuaranteedMonthsPay = Read("Guaranteed Months Pay"),
                    AnnualGuaranteedBonusDescription = Read("Other Guaranteed Bonuses (Received Annually)"),
                    AnnualGuaranteedBonusAmount = Read("Amount of Guaranteed Bonuses (Received Annually)"),
                    MonthlyAllowanceDescription = Read("Allowances (Received Monthly)"),
                    MonthlyAllowanceAmount = Read("Allowances (Received Monthly)1"),
                    NonMonthlyAllowanceDescription = Read("Other Allowances (Received Non-monthly)", "Other Allowances (Received Non-monthly; quarterly, semi-annual or annual basis )"),
                    NonMonthlyAllowanceAmount = Read("Amount of Other Allowances (Received Non-monthly)", "Amount of Other Allowances (Received Non-monthly; quarterly, semi-annual or annual basis )"),
                    MonthlyNonTaxableAllowanceDescription = Read("Fixed Non-Taxable Allowances (Received Monthly)"),
                    MonthlyNonTaxableAllowanceAmount = Read("Amount of Fixed Non-Taxable Allowances (Received Monthly)"),
                    AnnualNonTaxableAllowanceDescription = Read("Fixed Non-Taxable Allowances (Received Annually)"),
                    AnnualNonTaxableAllowanceAmount = Read("Amount of Fixed Non-Taxable Allowances (Received Annually)"),
                    AnnualProfitSharingAmount = Read("Profit Sharing"),
                    AnnualIncentiveDescription = Read("Total Incentives / Commission (Annual)"),
                    AnnualIncentiveAmount = Read("Total Incentives / Commission (Annual)1"),
                    AnnualVariablePayDescription = Read("Other Variable Pay (Annual)"),
                    AnnualVariablePayAmount = Read("Amount of Other Variable Pay (Annual)"),
                    EmployeeHmoBenefitLimit = Read("HMO Benefit Limit for Employee"),
                    DependentHmoBenefitLimit = Read("HMO Benefit Limit for Dependents"),
                    DentalBenefit = Read("Dental"),
                    MedicineReimbursementBenefit = Read("Medicine Reimbursement"),
                    OpticalBenefit = Read("Optical"),
                    OtherHealthBenefits = Read("Other Health Benefits"),
                    VacationLeaveBenefit = Read("Optional/Vacation Leaves"),
                    SickLeaveBenefit = Read("Sick Leave"),
                    OtherLeaveBenefits = Read("Other Leaves"),
                    LifeInsuranceBenefit = Read("Life Insurance"),
                    OtherBenefits = Read("Other Benefits not asked in the form", "Other Benefits you are receiving but not asked in this form"),
                    VehicleBenefit = Read("Car / Vehicle or Car Plan if received in cash"),
                    MobilePhoneBenefit = Read("Cellular Phone"),
                    CreatedAt = createdAt,
                    CreatedBy = createdBy
                });
            }

            return rawData;
        }

        private static string NormalizeHeader(string header)
        {
            return string.Join(" ", header.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries));
        }

        private void ResponseValidation(List<CandidateResponseRawData> responseRawData)
        {
            var emailValidator = new EmailAddressAttribute();
            foreach (var response in responseRawData)
            {
                var errors = new List<string>();

                if (string.IsNullOrWhiteSpace(response.CandidateDBoxID))
                {
                    errors.Add("CandidateDBoxID is required.");
                }

                void ValidateDate(string? value, string field)
                {
                    if (!DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces, out _)
                        && !DateTime.TryParse(value, CultureInfo.CurrentCulture, DateTimeStyles.AllowWhiteSpaces, out _))
                    {
                        errors.Add($"{field} must be a valid date/time.");
                    }
                }

                void ValidateAmount(string? value, string field)
                {
                    if (string.IsNullOrWhiteSpace(value))
                        return;

                    if (!decimal.TryParse(value, NumberStyles.Number, CultureInfo.InvariantCulture, out _))
                        errors.Add($"{field} must be a valid decimal amount.");
                }

                ValidateDate(response.ResponseStartedAt, nameof(response.ResponseStartedAt));
                ValidateDate(response.ResponseCompletedAt, nameof(response.ResponseCompletedAt));
                ValidateDate(response.FormCompletedDate, nameof(response.FormCompletedDate));

                if (string.IsNullOrWhiteSpace(response.EmailAddress)
                    || !emailValidator.IsValid(response.EmailAddress.Trim()))
                {
                    errors.Add("EmailAddress must be a valid email address.");
                }

                ValidateAmount(response.ExpectedMonthlyBasicSalary, nameof(response.ExpectedMonthlyBasicSalary));
                ValidateAmount(response.CurrentMonthlyBasicSalary, nameof(response.CurrentMonthlyBasicSalary));
                ValidateAmount(response.AnnualGuaranteedBonusAmount, nameof(response.AnnualGuaranteedBonusAmount));
                ValidateAmount(response.MonthlyAllowanceAmount, nameof(response.MonthlyAllowanceAmount));
                ValidateAmount(response.NonMonthlyAllowanceAmount, nameof(response.NonMonthlyAllowanceAmount));
                ValidateAmount(response.MonthlyNonTaxableAllowanceAmount, nameof(response.MonthlyNonTaxableAllowanceAmount));
                ValidateAmount(response.AnnualNonTaxableAllowanceAmount, nameof(response.AnnualNonTaxableAllowanceAmount));
                ValidateAmount(response.AnnualProfitSharingAmount, nameof(response.AnnualProfitSharingAmount));
                ValidateAmount(response.AnnualIncentiveAmount, nameof(response.AnnualIncentiveAmount));
                ValidateAmount(response.AnnualVariablePayAmount, nameof(response.AnnualVariablePayAmount));

                response.InvalidResponse = errors.Count > 0;
                response.ErrorMessage = errors.Count > 0 ? string.Join(Environment.NewLine, errors) : null;
            }
        }
    }
}
