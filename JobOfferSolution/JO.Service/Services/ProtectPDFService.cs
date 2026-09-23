using System.Text;
using iText.Kernel.Pdf;
using JO.Service.Services.Contracts;

namespace JO.Service.Services
{
    public class ProtectPDFService : IProtectPDFService
    {
        public byte[] ProtectPdf(byte[] pdfContent, string userPassword = "candidate123",
            string ownerPassword = "hrjoboffertool")
        {
            ArgumentNullException.ThrowIfNull(pdfContent);
            ArgumentException.ThrowIfNullOrWhiteSpace(userPassword);
            ArgumentException.ThrowIfNullOrWhiteSpace(ownerPassword);

            using var input = new MemoryStream(pdfContent);
            using var output = new MemoryStream();
            using (var reader = new PdfReader(input))
            using (var writer = new PdfWriter(output, new WriterProperties().SetStandardEncryption(
                Encoding.UTF8.GetBytes(userPassword),
                Encoding.UTF8.GetBytes(ownerPassword),
                EncryptionConstants.ALLOW_PRINTING,
                EncryptionConstants.ENCRYPTION_AES_256)))
            using (var document = new PdfDocument(reader, writer))
            {
                // Closing the document writes the encrypted copy.
            }

            return output.ToArray();
        }

        public void ProtectPdf(string pdfFilePath, string userPassword = "candidate123",
            string ownerPassword = "hrjoboffertool")
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(pdfFilePath);
            ArgumentException.ThrowIfNullOrWhiteSpace(userPassword);
            ArgumentException.ThrowIfNullOrWhiteSpace(ownerPassword);

            var sourcePath = Path.GetFullPath(pdfFilePath);
            if (!File.Exists(sourcePath))
                throw new FileNotFoundException("The PDF file was not found.", sourcePath);

            // Write beside the original so encryption failures leave the source intact.
            var temporaryPath = Path.Combine(Path.GetDirectoryName(sourcePath)!,
                $"{Guid.NewGuid():N}.tmp");
            try
            {
                using (var output = new FileStream(temporaryPath, FileMode.CreateNew, FileAccess.Write, FileShare.None))
                using (var reader = new PdfReader(sourcePath))
                using (var writer = new PdfWriter(output, new WriterProperties().SetStandardEncryption(
                    Encoding.UTF8.GetBytes(userPassword),
                    Encoding.UTF8.GetBytes(ownerPassword),
                    EncryptionConstants.ALLOW_PRINTING,
                    EncryptionConstants.ENCRYPTION_AES_256)))
                using (var document = new PdfDocument(reader, writer))
                {
                    // Closing the document writes the encrypted copy.
                }

                File.Move(temporaryPath, sourcePath, overwrite: true);
            }
            finally
            {
                if (File.Exists(temporaryPath))
                    File.Delete(temporaryPath);
            }
        }
    }
}
