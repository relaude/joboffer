using JO.DataModel.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace JO.DataModel.View
{
    public class VwTransactionAttachments
    {
        public int Id { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int Transaction_Id { get; set; }
        public int FileType_Id { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string TransactionNumber { get; set; }
        public string TypeName { get; set; }
    }
}
