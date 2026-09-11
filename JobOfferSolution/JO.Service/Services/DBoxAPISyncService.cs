using JO.DataModel.Entity;
using JO.Persistence.DataAccess;
using JO.Service.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;

namespace JO.Service.Services
{
    public class DBoxAPISyncService : IDBoxAPISyncService
    {
        private readonly IDbContextFactory<JobOfferDbContext> _dbContext;

        public DBoxAPISyncService(IDbContextFactory<JobOfferDbContext> dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<DboxCandidatesRawData>> GetDboxCandidatesRawData()
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            return await context.DboxCandidatesRawData.AsNoTracking()
                .OrderByDescending(candidate => candidate.Id).ToListAsync();
        }

        public async Task<List<DboxCandidatesRawData>> SaveDboxCandidatesRawData(Stream source, int createdBy)
        {
            ArgumentNullException.ThrowIfNull(source);
            using var stream = new MemoryStream();
            await source.CopyToAsync(stream);
            stream.Position = 0;

            ExcelPackage.License.SetNonCommercialOrganization("JobOffer");
            using var package = new ExcelPackage();
            await package.LoadAsync(stream);
            var candidates = ReadCandidates(package, createdBy);

            await using var context = await _dbContext.CreateDbContextAsync();
            await context.DboxCandidatesRawData.AddRangeAsync(candidates);
            await context.SaveChangesAsync();
            return candidates;
        }

        private static List<DboxCandidatesRawData> ReadCandidates(ExcelPackage package, int createdBy)
        {
            var candidates = new List<DboxCandidatesRawData>();
            var sheet = package.Workbook.Worksheets.FirstOrDefault();
            if (sheet?.Dimension is null)
                return candidates;

            static string NormalizeHeader(string header) =>
                string.Join(" ", header.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries));

            var columns = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            for (var column = 1; column <= sheet.Dimension.End.Column; column++)
            {
                var header = NormalizeHeader(sheet.Cells[1, column].Text);
                if (header.Length > 0)
                    columns.TryAdd(header, column);
            }

            var createdAt = DateTime.Now;
            for (var row = 2; row <= sheet.Dimension.End.Row; row++)
            {
                // Ignore empty template rows; save populated rows without business validation.
                if (!columns.Values.Any(column => !string.IsNullOrWhiteSpace(sheet.Cells[row, column].Text)))
                    continue;

                string? Read(string header)
                {
                    if (!columns.TryGetValue(header, out var column))
                        return null;
                    var value = sheet.Cells[row, column].Text.Trim();
                    return value.Length == 0 ? null : value;
                }

                candidates.Add(new DboxCandidatesRawData
                {
                    CandidateId = Read("Candidate ID"),
                    CandidateName = Read("Candidate Name"),
                    Company = Read("Company Code"),
                    Division = Read("Division Code"),
                    Department = Read("Department"),
                    CostCenter = Read("Cost Center"),
                    JobLevel = Read("Job Level"),
                    JobPosition = Read("Job Position"),
                    EmailAddress = Read("Email Address"),
                    ContactNumber = Read("Contact Number"),
                    CreatedAt = createdAt,
                    CreatedBy = createdBy
                });
            }
            return candidates;
        }
    }
}
