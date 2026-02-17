using System;
using System.Collections.Generic;
using System.Text;

namespace JO.DataModel.DTOs
{
    public class FileStreamDto
    {
        public string Name { get; set; }
        public string SizeInKb { get; set; }
        public byte[] Content { get; set; }
    }
}
