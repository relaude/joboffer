namespace JO.Service.Services.Contracts
{
    public interface IProtectPDFService
    {
        /// <summary>
        /// Returns an AES-256 password-protected copy of the PDF content.
        /// </summary>
        byte[] ProtectPdf(byte[] pdfContent, string userPassword = "candidate123",
            string ownerPassword = "hrjoboffertool");

        /// <summary>
        /// Password-protects an existing, unencrypted PDF in place using AES-256.
        /// The user password opens the PDF with printing permission; the owner password grants full access.
        /// The original file is preserved if encryption fails.
        /// Pass the absolute path returned by IJOFileService.SaveJobOfferFileAsync.
        /// </summary>
        void ProtectPdf(string pdfFilePath, string userPassword = "candidate123",
            string ownerPassword = "hrjoboffertool");
    }
}
