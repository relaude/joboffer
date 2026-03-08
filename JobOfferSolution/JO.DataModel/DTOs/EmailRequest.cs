using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace JO.DataModel.DTOs
{
    public class EmailRequest
    {
        public string To { get; set; }
        public string? Cc { get; set; }
        public string? Bcc { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public List<string>? AttachmentPaths { get; set; }
        public List<IFormFile>? Attachments { get; set; }
    }
}
