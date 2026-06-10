using JO.DataModel.Entity;
using JO.Persistence.DataAccess;
using JO.Service.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace JO.Service.Services
{
    public class MassUploadService : IMassUploadService
    {
        private readonly IDbContextFactory<JobOfferDbContext> _dbContext;
        private readonly IUtilitiesService _utils;
        public MassUploadService(IDbContextFactory<JobOfferDbContext> dbContext,
            IUtilitiesService utils)
        {
            _dbContext = dbContext;
            _utils = utils;
        }

        public async Task<List<CandidateTempData>> GetCandidateTempData()
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            return await context.CandidateTempData.AsNoTracking().ToListAsync();
        }

        public async Task<int> SaveExcelRowItems(Stream excelStream, int createdBy)
        {
            var newRecords = await TransferExcelItems(excelStream);

            if (newRecords.Any())
            {
                //mass upload logs
                var newLog = new CandidateMassUploadLogs
                {
                    TotalExcelItems = newRecords.Count(),
                    CreatedBy = createdBy,
                    CreatedAt = DateTime.Now
                };

                await using var context = await _dbContext.CreateDbContextAsync();

                //new log entry
                await context.CandidateMassUploadLogs.AddAsync(newLog);
                await context.SaveChangesAsync();

                //assign log id
                foreach (var record in newRecords)
                    record.MassUploadLogId = newLog.Id;

                //save excel raw data
                List<CandidateExcelRawData> excelRawData = new();
                foreach (var record in newRecords)
                {
                    excelRawData.Add(new CandidateExcelRawData
                    {
                        FirstName = record.FirstName,
                        MiddleName = record.MiddleName,
                        LastName = record.LastName,
                        Email = record.Email,
                        ContactNumber = record.ContactNumber,
                        PositionAppliedFor = record.PositionAppliedFor,
                        ExpectedSalary = record.ExpectedSalary,
                        MassUploadLogId = record.MassUploadLogId,
                        CreatedBy = createdBy,
                        CreatedAt = DateTime.Now
                    });
                }
                await context.CandidateExcelRawData.AddRangeAsync(excelRawData);

                //delete old data then save new data
                await context.CandidateTempData.ExecuteDeleteAsync();
                await context.CandidateTempData.AddRangeAsync(newRecords);

                //execute sql transactions
                return await context.SaveChangesAsync();
            }

            return 0;
        }

        private async Task<IEnumerable<CandidateTempData>> TransferExcelItems(Stream excelStream, CancellationToken cancellationToken = default)
        {
            if (excelStream is null)
            {
                throw new ArgumentNullException(nameof(excelStream));
            }

            if (excelStream.CanSeek)
            {
                excelStream.Position = 0;
            }

            ExcelPackage.License.SetNonCommercialOrganization("JobOffer");
            using var package = new ExcelPackage();
            await package.LoadAsync(excelStream, cancellationToken);

            var worksheet = package.Workbook.Worksheets.FirstOrDefault();
            if (worksheet?.Dimension is null)
            {
                return Enumerable.Empty<CandidateTempData>();
            }

            var columns = GetHeaderColumns(worksheet);
            var items = new List<CandidateTempData>();

            for (var row = worksheet.Dimension.Start.Row + 1; row <= worksheet.Dimension.End.Row; row++)
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (IsRowEmpty(worksheet, row, worksheet.Dimension.Start.Column, worksheet.Dimension.End.Column))
                {
                    continue;
                }

                var errors = new List<string>();

                items.Add(new CandidateTempData
                {
                    FirstName = await GetCellText(worksheet, row, columns, "FirstName", errors),
                    MiddleName = await GetCellText(worksheet, row, columns, "MiddleName", errors),
                    LastName = await GetCellText(worksheet, row, columns, "LastName", errors),
                    Email = await GetCellEmailText(worksheet, row, columns, "Email", errors),
                    ContactNumber = await GetCellText(worksheet, row, columns, "ContactNumber", errors),
                    PositionAppliedFor = await GetCellText(worksheet, row, columns, "PositionApplied", errors),
                    ExpectedSalary = GetCellDecimalText(worksheet, row, columns, "ExpectedSalary", errors),

                    HasErrors = errors.Any(),
                    Errors = errors.Any() ? string.Join(Environment.NewLine, errors) : null
                });
            }

            return items;
        }

        private static Dictionary<string, int> GetHeaderColumns(ExcelWorksheet worksheet)
        {
            var headers = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            var headerRow = worksheet.Dimension.Start.Row;

            for (var column = worksheet.Dimension.Start.Column; column <= worksheet.Dimension.End.Column; column++)
            {
                var header = worksheet.Cells[headerRow, column].Text.Trim();
                if (!string.IsNullOrWhiteSpace(header))
                {
                    headers[header] = column;
                }
            }

            return headers;
        }

        private static bool IsRowEmpty(ExcelWorksheet worksheet, int row, int startColumn, int endColumn)
        {
            for (var column = startColumn; column <= endColumn; column++)
            {
                if (!string.IsNullOrWhiteSpace(worksheet.Cells[row, column].Text))
                {
                    return false;
                }
            }

            return true;
        }

        private async Task<string?> GetCellText(ExcelWorksheet worksheet, int row, Dictionary<string, int> columns, string header, List<string> errors)
        {
            string? cellValue = columns.TryGetValue(header, out var column)
                ? EmptyToNull(worksheet.Cells[row, column].Text)
                : null;

            if (string.IsNullOrEmpty(cellValue))
            { errors.Add($"{header} is not a valid input."); }
            else
            {
                await using var context = await _dbContext.CreateDbContextAsync();

                if (header == "PositionApplied")
                {
                    var jobPosition = await context.JobPositions.FirstOrDefaultAsync(jo => jo.PositionName == cellValue);
                    if (jobPosition == null)
                        errors.Add($"{header} - {cellValue} dont exists in DB.");
                }
            }

            return cellValue;
        }

        private static string? EmptyToNull(string value)
        {
            var trimmedValue = value.Trim();
            return string.IsNullOrWhiteSpace(trimmedValue) ? null : trimmedValue;
        }

        private async Task<string?> GetCellEmailText(ExcelWorksheet worksheet, int row, Dictionary<string, int> columns, string header, List<string> errors)
        {
            string? cellValue = columns.TryGetValue(header, out var column)
                ? EmptyToNull(worksheet.Cells[row, column].Text)
                : null;

            bool validEmail = _utils.IsValidEmail(cellValue);

            if(validEmail)
            {
                await using var context = await _dbContext.CreateDbContextAsync();
                var candidate = await context.Candidates.FirstOrDefaultAsync(jo => jo.Email == cellValue);
                if (candidate != null)
                    errors.Add($"{header} - {cellValue} exists in DB.");
            }

            if (!validEmail)
                errors.Add($"{header} is not a valid email.");

            return cellValue;
        }

        private decimal? GetCellDecimalText(ExcelWorksheet worksheet, int row, Dictionary<string, int> columns, string header, List<string> errors)
        {
            string? cellValue = columns.TryGetValue(header, out var column)
                ? EmptyToNull(worksheet.Cells[row, column].Text)
                : null;

            if (!decimal.TryParse(cellValue, out var decimalValue))
                errors.Add($"{header} is not a valid input.");

            return decimalValue;
        }
    }
}
