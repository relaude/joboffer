using JO.DataModel.Entity;
using JO.Persistence.DataAccess;
using JO.Service.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace JO.Service.Services
{
    public class CandidateFileService : ICandidateFileService
    {
        private readonly IDbContextFactory<JobOfferDbContext> _dbContext;
        public const long MaxFileSize = 3 * 1024 * 1024;
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<CandidateFileService> _logger;

        public CandidateFileService(IDbContextFactory<JobOfferDbContext> dbContext,
            IWebHostEnvironment environment, ILogger<CandidateFileService> logger)
        {
            _dbContext = dbContext;
            _environment = environment;
            _logger = logger;
        }

        public async Task<List<DocumentType>> GetDocumentType()
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            return await context.DocumentType.AsNoTracking().OrderBy(type => type.TypeName).ToListAsync();
        }

        public async Task<DboxCandidates?> GetCandidateById(int candidateId)
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            return await context.DboxCandidates.AsNoTracking().SingleOrDefaultAsync(c => c.Id == candidateId);
        }

        public async Task<List<CandidateFiles>> GetCandidateFiles(int candidateId)
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            return await context.CandidateFiles.AsNoTracking().Where(f => f.CandidateId == candidateId)
                .OrderByDescending(f => f.CreatedAt).ThenByDescending(f => f.Id).ToListAsync();
        }

        public async Task<int> InsertCandidateFiles(CandidateFiles file, Stream content)
        {
            ArgumentNullException.ThrowIfNull(file);
            ArgumentNullException.ThrowIfNull(content);
            if (file.Id != 0 || file.CandidateId.GetValueOrDefault() <= 0)
                throw new InvalidOperationException("A valid candidate is required for a new file.");
            await using var context = await _dbContext.CreateDbContextAsync();
            var candidate = await context.DboxCandidates.AsNoTracking()
                .SingleOrDefaultAsync(c => c.Id == file.CandidateId)
                ?? throw new InvalidOperationException("Candidate was not found.");
            if (!await context.DocumentType.AnyAsync(t => t.Id == file.TypeId))
                throw new InvalidOperationException("Select a valid document type.");
            var reference = candidate.DboxRefNum;
            if (string.IsNullOrWhiteSpace(reference) || reference is "." or ".."
                || reference.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0
                || reference.Contains('/') || reference.Contains('\\') || reference.EndsWith('.') || reference.EndsWith(' '))
                throw new InvalidOperationException("The candidate needs a valid DBox reference number.");
            var name = Path.GetFileName(file.FileName?.Replace('\\', '/'));
            if (string.IsNullOrWhiteSpace(name) || name is "." or ".." || name.Length > 200)
                throw new InvalidOperationException("Choose a file with a valid name of at most 200 characters.");

            // Enforce the limit on actual bytes as well as the browser's declared size.
            using var buffer = new MemoryStream();
            var chunk = new byte[81920];
            int count;
            while ((count = await content.ReadAsync(chunk)) > 0)
            {
                if (buffer.Length + count > MaxFileSize)
                    throw new InvalidOperationException("File size must not exceed 3 MB.");
                await buffer.WriteAsync(chunk.AsMemory(0, count));
            }
            if (buffer.Length == 0)
                throw new InvalidOperationException("The selected file is empty.");

            var root = GetDocsRoot();
            var directory = Path.Combine(root, reference);
            Directory.CreateDirectory(directory);
            // Prefix the original filename with a GUID to avoid collisions.
            var path = Path.Combine(directory, $"{Guid.NewGuid():N}_{name}");
            try
            {
                await using (var output = new FileStream(path, FileMode.CreateNew, FileAccess.Write, FileShare.None))
                {
                    buffer.Position = 0;
                    await buffer.CopyToAsync(output);
                }
                file.FileName = name;
                file.RelativePath = Path.GetRelativePath(_environment.WebRootPath, path).Replace('\\', '/');
                file.CreatedAt = DateTime.Now;
                context.CandidateFiles.Add(file);
                await context.SaveChangesAsync();
                return file.Id;
            }
            catch
            {
                try { File.Delete(path); }
                catch (Exception ex) { _logger.LogWarning(ex, "Unable to clean up candidate file {Path}", path); }
                throw;
            }
        }

        public async Task<(string Path, string FileName)?> GetDownload(int candidateId, int fileId)
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            var file = await context.CandidateFiles.AsNoTracking()
                .SingleOrDefaultAsync(f => f.Id == fileId && f.CandidateId == candidateId);
            if (file is null || string.IsNullOrWhiteSpace(file.RelativePath)) return null;
            var root = GetDocsRoot() + Path.DirectorySeparatorChar;
            var path = Path.GetFullPath(Path.Combine(_environment.WebRootPath, file.RelativePath));
            if (!path.StartsWith(root, OperatingSystem.IsWindows() ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal)
                || !File.Exists(path)) return null;
            return (path, file.FileName ?? "download");
        }

        private string GetDocsRoot()
        {
            if (string.IsNullOrWhiteSpace(_environment.WebRootPath))
                throw new InvalidOperationException("The web root directory is not configured.");
            return Path.GetFullPath(Path.Combine(_environment.WebRootPath, "docs"));
        }
    }
}
